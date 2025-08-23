using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>
    {
        Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<PurchaseOrder>> GetBySupplierIdAsync(Guid supplierId);
        Task<PurchaseOrder?> GetWithItemsAsync(Guid purchaseOrderId);
        Task<decimal> GetTotalAmountForSupplierAsync(Guid supplierId);
        Task<decimal> GetTotalOutstandingAmountAsync(Guid purchaseOrderId);
        Task UpdateStatusAsync(PurchaseOrder purchaseOrder);
        Task<int> CountByStatusAsync(PurchaseOrderStatus status);
    }


}
