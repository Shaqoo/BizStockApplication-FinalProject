using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly BizStockContext _context;

        public InvoiceRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
        {
            return await _context.Invoices
                .Include(a => a.Customer)
                .Include(a => a.Items)
                .Include(a => a.SalesOrder)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<Invoice>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Invoices
                .Include(a => a.Customer)
                .Include(a => a.Items)
                .Include(a => a.SalesOrder)
                .OrderByDescending(i => i.DateCreated);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Invoice>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Invoice>> FindAsync(Expression<Func<Invoice, bool>> predicate)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<PaginatedList<Invoice>> GetByCustomerIdAsync(Guid customerId, PageRequest pageRequest)
        {
            var query = _context.Invoices
                .Include(a => a.Customer)
                .Include(a => a.Items)
                .Include(a => a.SalesOrder)
                .Where(i => i.CustomerId == customerId)
                .OrderByDescending(i => i.DateCreated);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Invoice>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Invoice>> GetUnpaidInvoicesAsync(Guid customerId)
        {
            return await _context.Invoices
                .Where(i => i.CustomerId == customerId && i.Status != InvoiceStatus.Paid)
                .ToListAsync();
        }

        public async Task<PaginatedList<Invoice>> GetOverdueInvoicesAsync(PageRequest pageRequest)
        {
            var now = DateTime.UtcNow;

            var query = _context.Invoices
                .Where(i => i.DueDate < now && i.Status != InvoiceStatus.Paid)
                .OrderBy(i => i.DueDate);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Invoice>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<decimal> GetTotalOutstandingAsync(Guid customerId)
        {
            return await _context.Invoices
                .Where(i => i.CustomerId == customerId && i.Status != InvoiceStatus.Paid)
                .SumAsync(i => i.TotalAmount);
        }

        public async Task<bool> IsInvoicePaidAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            return invoice != null && invoice.Status == InvoiceStatus.Paid;
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await Task.CompletedTask;
        }

        public async Task DeleteInvoiceAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await Task.CompletedTask;
        }

        public async Task<Invoice?> GetByExpression(Expression<Func<Invoice, bool>> predicate)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByOrderIdAsync(Guid orderId)
        {
            return await _context.Invoices
                .Include(a => a.Customer)
                .Include(a => a.Items)
                .Include(a => a.SalesOrder)
                .Where(i => i.SalesOrderId == orderId)
                .ToListAsync();
        }
    }

}
