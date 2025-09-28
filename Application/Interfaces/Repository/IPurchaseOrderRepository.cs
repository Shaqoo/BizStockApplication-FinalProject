using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repository
{
    public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>
    {
        Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<string> GenerateNextOrderNumber();
        Task<PaginatedList<PurchaseOrderListDto>> GetBySupplierIdAsync(Guid supplierId,PageRequest pageRequest);
        Task<PurchaseOrder?> GetWithItemsAsync(Guid purchaseOrderId);
        Task<decimal> GetTotalAmountForSupplierAsync(Guid supplierId);
        Task<decimal> GetTotalOutstandingAmountAsync(Guid purchaseOrderId);
        Task UpdateStatusAsync(PurchaseOrder purchaseOrder);
        Task<int> CountByStatusAsync(PurchaseOrderStatus status);
        Task<PurchaseOrderDetailDto?> GetPurchaseOrderDetailsById(Guid purchaseOrderId);
        Task<PaginatedList<PurchaseOrderListDto>> GetAllWithDtoAsync(PageRequest pageRequest);
        Task<PaginatedList<PurchaseOrderListDto>> FilterPurchaseOrderWithStatusPagedAsync(PurchaseOrderStatus purchaseOrderStatus,PageRequest pageRequest);
        Task<PurchaseOrderStatsDto> GetPurchaseOrderStatsAsync();
        Task<PoTrendDto> GetMonthlyPurchaseOrderTrendsAsync(int months = 6);
        Task<PaginatedList<PurchaseOrderListDto>> GetPurchaseOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, PageRequest pageRequest);
    }


}
