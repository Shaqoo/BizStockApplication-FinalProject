using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly BizStockContext _context;

        public CartItemRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetByIdAsync(Guid itemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product) 
                .FirstOrDefaultAsync(ci => ci.Id == itemId);
        }

        public async Task<List<CartItem>> GetByCartIdAsync(Guid cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }

        public async Task AddAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<CartItem>> GetPaginatedAsync(Guid cartId, PageRequest pageRequest)
        {
            var query = _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product) 
                .AsNoTracking();

            var count = await query.CountAsync();

            var items = await query
                .OrderBy(ci => ci.ProductId) 
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<CartItem>(items, count, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<decimal> GetItemTotalAsync(Guid itemId)
        {
            var item = await _context.CartItems
                .Where(ci => ci.Id == itemId)
                .Include(ci => ci.Product)
                .Select(ci => ci.Product.SellingPrice * ci.Quantity)
                .FirstOrDefaultAsync();

            return item;
        }

        public async Task<CartItem?> GetByExpression(Expression<Func<CartItem, bool>> expression)
        {
            return await _context.CartItems.FirstOrDefaultAsync(expression);
        }
    }

}
