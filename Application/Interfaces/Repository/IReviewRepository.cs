using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        Task UpdateAsync (Review review);
        Task<IEnumerable<Review>> GetByReviewerIdAsync(Guid reviewerId);
        Task<PaginatedList<Review>> GetByProductIdAsync(Guid productId,PageRequest pageRequest);
        Task<IEnumerable<Review>> GetBySupplierIdAsync(Guid supplierId);
        Task<IEnumerable<Review>> GetByDeliveryAgentIdAsync(Guid deliveryAgentId);
        Task<int> TotalRatingForAProductAsync(Guid productId);
        Task<double> GetAverageRatingForProductAsync(Guid productId);
        Task<double> GetAverageRatingForSupplierAsync(Guid supplierId);
        Task<double> GetAverageRatingForDeliveryAgentAsync(Guid deliveryAgentId);
        Task HideReviewAsync(Guid reviewId);
        Task ShowReviewAsync(Guid reviewId);
        Task<RatingSummaryDto> GetProductRatingSummaryAsync(Guid productId);
    }

}
