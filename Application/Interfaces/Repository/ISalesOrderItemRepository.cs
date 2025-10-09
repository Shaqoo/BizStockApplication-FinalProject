using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ISalesOrderItemRepository : IBaseRepository<SalesOrderItem>
    {
        Task<IEnumerable<SalesOrderItem>> GetBySalesOrderIdAsync(Guid salesOrderId);
        Task<IEnumerable<SalesOrderItem>> GetByProductIdAsync(Guid productId);
        Task<decimal> GetTotalSalesForProductAsync(Guid productId);
        Task<int> GetTotalUnitsSoldForProductAsync(Guid productId);
        Task<PaginatedList<SalesOrderItem>> GetByDateRangeAsync(DateTime start, DateTime end,PageRequest pageRequest);
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int topN);
        Task<IEnumerable<SalesOrderItem>> GetPendingOrInTransitAsync();
        Task UpdateAsync(SalesOrderItem salesOrderItem);
    }


}
