using Application.Dto;
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
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task AddRangeAsync(IEnumerable<CartItem> cartItems)
        {
            await _context.CartItems.AddRangeAsync(cartItems);
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

        public async Task<PaginatedList<CartItemDto>> GetCartItemsAsync(Guid cartId, PageRequest pageRequest)
        {
            var query = _context.CartItems
                .Where(ci => ci.CartId == cartId).AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(ci => ci.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CartId = ci.CartId
                })
                .ToListAsync();
            return new PaginatedList<CartItemDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<CartItemDto>> GetCartItemsByUserIdAsync(Guid userId, PageRequest pageRequest)
        {
           var query = _context.CartItems.Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId).AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(ci => ci.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CartId = ci.CartId
                })
                .ToListAsync();
            return new PaginatedList<CartItemDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<CartItemDto>> GetCartItemsBySessionIdAsync(string sessionId, PageRequest pageRequest)
        {
            var query = _context.CartItems.Include(ci => ci.Cart)
                .Where(ci => ci.Cart.SessionId == sessionId).AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(ci => ci.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(ci => new CartItemDto
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    CartId = ci.CartId
                })
                .ToListAsync();
            return new PaginatedList<CartItemDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }
    }

}
