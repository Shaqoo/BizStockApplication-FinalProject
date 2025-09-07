using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BizStockContext _context;

        public WishlistRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?> GetByIdAsync(Guid wishlistId)
        {
            return await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.Id == wishlistId);
        }

        public async Task<Wishlist?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Wishlists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
        public async Task AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
        }

        public async Task AddRangeAsync(IEnumerable<WishlistItem> wishlists)
        {
            await _context.WishlistItems.AddRangeAsync(wishlists);
        }

        public async Task UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<WishlistItemDto>> GetAllByUserAsync(PageRequest pageRequest, Guid userId)
        {
            var query = _context.WishlistItems
                    .AsNoTracking()
                    .Where(wi => wi.Wishlist.UserId == userId)
                    .Select(wi => new WishlistItemDto
                    {
                        Id = wi.Id,
                        WishlistId = wi.WishlistId,
                        ProductId = wi.ProductId,
                        ProductName = wi.Product.Name,
                        BrandName = wi.Product.Brand.Name,
                        ProductPrice = wi.Product.SellingPrice,
                        ProductImageUrl = wi.Product.ImageUrl,
                        CreatedAt = wi.CreatedAt
                    })
                    .OrderByDescending(wi => wi.CreatedAt);


            var pagedQuery = await query
                .OrderByDescending(wi => wi.CreatedAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize).ToListAsync();

            return new PaginatedList<WishlistItemDto>(
                pagedQuery,
                query.Count(),
                pageRequest.Page,
                pageRequest.PageSize
            );
        }

        public async Task AddItemsAsync(WishlistItem wishlist)
        {
            await _context.WishlistItems.AddAsync(wishlist);
        }

        public async Task<bool> CheckIfItemExists(Guid userId, Guid productId)
        {
             return await _context.WishlistItems
                .AsNoTracking()
                .AnyAsync(wi => wi.Wishlist.UserId == userId && wi.ProductId == productId);
        }
    }

}
