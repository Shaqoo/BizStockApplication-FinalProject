using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BizStockContext _context;

        public ReviewRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review entity)
        {
            await _context.Reviews.AddAsync(entity);
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews.FindAsync(id)
                ?? throw new KeyNotFoundException("Review not found.");
        }

        public async Task<PaginatedList<Review>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Reviews.AsQueryable();

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.ReviewedAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Review>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Review>> FindAsync(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Reviews.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId)
        {
            return await _context.Reviews
                .Where(r => r.ReviewerId == reviewerId)
                .ToListAsync();
        }

        public async Task<PaginatedList<Review>> GetByProductIdAsync(Guid productId,PageRequest pageRequest)
        {
            var query = _context.Reviews
                .Include(a => a.Reviewer)
                .Where(r => r.ProductId == productId && r.IsVisible);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.ReviewedAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Review>(items, total, pageRequest.Page, pageRequest.PageSize);

        }

        public async Task<IEnumerable<Review>> GetBySupplierIdAsync(Guid supplierId)
        {
            return await _context.Reviews
                .Where(r => r.SupplierId == supplierId && r.IsVisible)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByDeliveryAgentIdAsync(Guid deliveryAgentId)
        {
            return await _context.Reviews
                .Where(r => r.DeliveryAgentId == deliveryAgentId && r.IsVisible)
                .ToListAsync();
        }

        public async Task<int> TotalRatingForAProductAsync(Guid productId)
        {
            return await _context.Reviews.CountAsync(a => a.ProductId == productId);
        }

        public async Task<double> GetAverageRatingForProductAsync(Guid productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsVisible)
                .Select(r => (double?)r.Rating)
                .AverageAsync() ?? 0.0;
        }

        public async Task<double> GetAverageRatingForSupplierAsync(Guid supplierId)
        {
            return await _context.Reviews
                .Where(r => r.SupplierId == supplierId && r.IsVisible)
                .Select(r => (double?)r.Rating)
                .AverageAsync() ?? 0.0;
        }

        public async Task<double> GetAverageRatingForDeliveryAgentAsync(Guid deliveryAgentId)
        {
            return await _context.Reviews
                .Where(r => r.DeliveryAgentId == deliveryAgentId && r.IsVisible)
                .Select(r => (double?)r.Rating)
                .AverageAsync() ?? 0.0;
        }

        public async Task HideReviewAsync(Guid reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId)
                ?? throw new KeyNotFoundException("Review not found.");

             review.Hide();

            _context.Reviews.Update(review);
        }

        public async Task ShowReviewAsync(Guid reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId)
                ?? throw new KeyNotFoundException("Review not found.");

            review.Show();

            _context.Reviews.Update(review);
        }

        public async Task<Review?> GetByExpression(Expression<Func<Review, bool>> predicate)
        {
            return await _context.Reviews.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await Task.CompletedTask;
        }

        public async Task<RatingSummaryDto> GetProductRatingSummaryAsync(Guid productId)
        {
            var total = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .CountAsync();

            if (total == 0)
            {
                return new RatingSummaryDto
                {
                    Average = 0.0,
                    Total = 0,
                    Breakdown = new Dictionary<int, int>
                    {
                        { 5, 0 }, { 4, 0 }, { 3, 0 }, { 2, 0 }, { 1, 0 }
                    }
                };
            }

            var average = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => r.Rating);

            var breakdownList = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .GroupBy(r => r.Rating)
                .Select(g => new { Rating = g.Key, Count = g.Count() })
                .ToListAsync();

            var breakdown = Enumerable.Range(1, 5)
                .ToDictionary(i => i, i => breakdownList.FirstOrDefault(b => b.Rating == i)?.Count ?? 0);

            return new RatingSummaryDto
            {
                Average = Math.Round(average, 1),
                Total = total,
                Breakdown = breakdown
            };
        }

    }

}
