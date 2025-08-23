using Domain.Entities;
using Domain.Entities.Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IRecentlyViewedProductRepository
    {
        Task<RecentlyViewedProducts?> GetByIdAsync(Guid id);
        Task<RecentlyViewedProducts?> GetByUserIdAsync(Guid userId);
        Task<RecentlyViewedProducts?> GetBySessionIdAsync(string sessionId);
        Task AddAsync(RecentlyViewedProducts item);
        Task AddRangeAsync(IEnumerable<RecentlyViewedProduct> items);
        Task UpdateAsync(RecentlyViewedProducts item);
        Task DeleteAsync(RecentlyViewedProducts item);
    }

}
