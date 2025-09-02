using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly BizStockContext _context;

        public CartRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByIdAsync(Guid cartId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product) 
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await Task.CompletedTask;
        }

        public async Task<decimal> GetTotalPriceAsync(Guid cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product) 
                .SumAsync(ci => ci.Product.SellingPrice * ci.Quantity);
        }

        public async Task<decimal> GetTotalCountAsync(Guid cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .SumAsync(ci => ci.Quantity);
        }

        public async Task<PaginatedList<CartItem>> GetCartItemsAsync(Guid cartId, PageRequest pageRequest)
        {
            var query = _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Product) 
                .AsQueryable();

            var count = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<CartItem>(items, count, pageRequest.Page, pageRequest.PageSize);
        }
    }
}


