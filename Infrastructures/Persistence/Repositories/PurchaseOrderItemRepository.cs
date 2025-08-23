using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class PurchaseOrderItemRepository : IPurchaseOrderItemRepository
    {
        private readonly BizStockContext _context;

        public PurchaseOrderItemRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PurchaseOrderItem entity)
        {
            await _context.PurchaseOrderItems.AddAsync(entity);
        }

        public async Task<PurchaseOrderItem?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrderItems.FindAsync(id)
                ?? throw new KeyNotFoundException("Purchase order item not found.");
        }

        public async Task<PaginatedList<PurchaseOrderItem>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.PurchaseOrderItems.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<PurchaseOrderItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<PurchaseOrderItem>> FindAsync(Expression<Func<PurchaseOrderItem, bool>> predicate)
        {
            return await _context.PurchaseOrderItems.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .Where(i => i.PurchaseOrderId == purchaseOrderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PurchaseOrderItem>> GetPendingItemsAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .Where(i => i.PurchaseOrderId == purchaseOrderId && i.QuantityReceived < i.QuantityOrdered)
                .ToListAsync();
        }

        public async Task<int> CountFullyReceivedItemsAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .CountAsync(i => i.PurchaseOrderId == purchaseOrderId && i.QuantityReceived >= i.QuantityOrdered);
        }

        public async Task<int> CountPendingItemsAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .CountAsync(i => i.PurchaseOrderId == purchaseOrderId && i.QuantityReceived < i.QuantityOrdered);
        }

        public async Task UpdateQuantityReceivedAsync(Guid itemId, int quantityReceived)
        {
            var item = await _context.PurchaseOrderItems.FindAsync(itemId)
                ?? throw new KeyNotFoundException("Purchase order item not found.");

            item.Receive(quantityReceived);

            _context.PurchaseOrderItems.Update(item);
        }

        public async Task<decimal> GetTotalAmountForPurchaseOrderAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrderItems
                .Where(i => i.PurchaseOrderId == purchaseOrderId)
                .SumAsync(i => i.TotalPrice);
        }

        public async Task<PurchaseOrderItem> GetByExpression(Expression<Func<PurchaseOrderItem, bool>> predicate)
        {
            return await _context.PurchaseOrderItems.FirstOrDefaultAsync(predicate) ??
               throw new ArgumentNullException("Purchase Order Item Not Found");
        }
    }

}
