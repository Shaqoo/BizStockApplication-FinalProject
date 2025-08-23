using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IPurchaseOrderItemRepository : IBaseRepository<PurchaseOrderItem>
    {
        Task<IEnumerable<PurchaseOrderItem>> GetByPurchaseOrderIdAsync(Guid purchaseOrderId);
        Task<IEnumerable<PurchaseOrderItem>> GetPendingItemsAsync(Guid purchaseOrderId);
        Task<int> CountFullyReceivedItemsAsync(Guid purchaseOrderId);
        Task<int> CountPendingItemsAsync(Guid purchaseOrderId);
        Task UpdateQuantityReceivedAsync(Guid itemId, int quantityReceived);
        Task<decimal> GetTotalAmountForPurchaseOrderAsync(Guid purchaseOrderId);
    }

}
