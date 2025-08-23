using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BizStockContext _context;

        public PaymentRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments.FindAsync(id)
                ?? throw new KeyNotFoundException("Payment not found.");
        }

        public async Task<PaginatedList<Payment>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Payments
                .Include(p => p.Invoice)
                .Include(p => p.Payer)
                .OrderByDescending(p => p.Id);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Payment>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Payment>> FindAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _context.Payments
                .Include(p => p.Invoice)
                .Include(p => p.Payer)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Payment?> GetByReferenceAsync(string paymentReference)
        {
            return await _context.Payments
                .Include(p => p.Invoice)
                .FirstOrDefaultAsync(p => p.PaymentReference == paymentReference);
        }

        public async Task<IEnumerable<Payment>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.Payments
                .Where(p => p.InvoiceId == invoiceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPayerIdAsync(Guid payerId)
        {
            return await _context.Payments
                .Where(p => p.PayerId == payerId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaidForInvoiceAsync(Guid invoiceId)
        {
            return await _context.Payments
                .Where(p => p.InvoiceId == invoiceId && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);
        }

        public async Task<bool> IsInvoiceFullyPaidAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return false;

            var totalPaid = invoice.Payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            return totalPaid >= invoice.TotalAmount;
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.Status == PaymentStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await Task.CompletedTask;
        }

        public async Task<Payment> GetByExpression(Expression<Func<Payment, bool>> predicate)
        {
            return await _context.Payments.FirstOrDefaultAsync(predicate) ??
               throw new ArgumentNullException("Payment Not Found");
        }
    }

}
