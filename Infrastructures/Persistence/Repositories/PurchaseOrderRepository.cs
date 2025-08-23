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
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly BizStockContext _context;

        public PurchaseOrderRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PurchaseOrder entity)
        {
            await _context.PurchaseOrders.AddAsync(entity);
        }

        public async Task<PurchaseOrder?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders.FindAsync(id)
                ?? throw new KeyNotFoundException("Purchase order not found.");
        }

        public async Task<PaginatedList<PurchaseOrder>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.PurchaseOrders.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<PurchaseOrder>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<PurchaseOrder>> FindAsync(Expression<Func<PurchaseOrder, bool>> predicate)
        {
            return await _context.PurchaseOrders.Where(predicate).ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.PurchaseOrders.FirstOrDefaultAsync(p => p.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId)
        {
            return await _context.PurchaseOrders
                .Where(p => p.SupplierId == supplierId)
                .OrderByDescending(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetWithItemsAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);
        }

        public async Task<decimal> GetTotalAmountForSupplierAsync(Guid supplierId)
        {
            return await _context.PurchaseOrders
                .Where(p => p.SupplierId == supplierId)
                .SumAsync(p => p.Total);
        }

        public async Task<decimal> GetTotalOutstandingAmountAsync(Guid purchaseOrderId)
        {
            var po = await _context.PurchaseOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);

            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            var receivedAmount = po.Items.Sum(i => i.QuantityReceived * i.UnitPrice);
            return po.Total - receivedAmount;
        }

        public async Task UpdateStatusAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
            await Task.CompletedTask;
        }

        public async Task<int> CountByStatusAsync(PurchaseOrderStatus status)
        {
            return await _context.PurchaseOrders.CountAsync(p => p.Status == status);
        }

        public async Task<PurchaseOrder> GetByExpression(Expression<Func<PurchaseOrder, bool>> predicate)
        {
            return await _context.PurchaseOrders.FirstOrDefaultAsync(predicate) ??
                throw new ArgumentNullException("Purchase Order Not Found");
        }
    }

}
