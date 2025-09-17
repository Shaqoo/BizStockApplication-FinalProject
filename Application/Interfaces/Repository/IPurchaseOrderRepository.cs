using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repository
{
    public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>
    {
        Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<string> GenerateNextOrderNumber();
        Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId);
        Task<PurchaseOrder?> GetWithItemsAsync(Guid purchaseOrderId);
        Task<decimal> GetTotalAmountForSupplierAsync(Guid supplierId);
        Task<decimal> GetTotalOutstandingAmountAsync(Guid purchaseOrderId);
        Task UpdateStatusAsync(PurchaseOrder purchaseOrder);
        Task<int> CountByStatusAsync(PurchaseOrderStatus status);
    }


}
