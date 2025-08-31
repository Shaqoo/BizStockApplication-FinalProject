using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence.Repositories
{
    public class RecentlyViewedProductRepository : IRecentlyViewedProductRepository
    {
        private readonly BizStockContext _context;

        public RecentlyViewedProductRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<RecentlyViewedProducts?> GetByIdAsync(Guid id)
        {
            return await _context.RecentlyViewedProducts
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RecentlyViewedProducts?> GetByUserIdAsync(Guid userId)
        {
            return await _context.RecentlyViewedProducts
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<RecentlyViewedProducts?> GetBySessionIdAsync(string sessionId)
        {
            return await _context.RecentlyViewedProducts
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.SessionId == sessionId);
        }

        public async Task AddAsync(RecentlyViewedProducts item)
        {
            await _context.RecentlyViewedProducts.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<RecentlyViewedProduct> items)
        {
            await _context.RecentlyViewedProductsItems.AddRangeAsync(items);
        }

        public async Task UpdateAsync(RecentlyViewedProducts item)
        {
            _context.RecentlyViewedProducts.Update(item);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(RecentlyViewedProducts item)
        {
            _context.RecentlyViewedProducts.Remove(item);
            await Task.CompletedTask;
        }

        public async Task AddProductAsync(RecentlyViewedProduct item)
        {
            await _context.RecentlyViewedProductsItems.AddAsync(item);
        }

        public async Task DeleteItemAsync(RecentlyViewedProduct recentlyViewedProduct)
        {
            _context.RecentlyViewedProductsItems.Remove(recentlyViewedProduct);
            await Task.CompletedTask;
        }
    }

}
