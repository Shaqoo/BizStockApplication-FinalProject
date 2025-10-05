using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IDeliveryAssignmentRepository : IBaseRepository<DeliveryAssignment>
    {
        Task<DeliveryAssignment?> GetBySalesOrderIdAsync(Guid salesOrderId);
        Task UpdateDeliveryAssignment(DeliveryAssignment deliveryAssignment);
        Task<PaginatedList<DeliveryAssignment>> GetByDeliveryAgentIdAsync(Guid deliveryAgentId,PageRequest pageRequest);
        Task<IEnumerable<DeliveryAssignment>> GetPendingDeliveriesAsync(Guid deliveryAgentId);
        Task<PaginatedList<DeliveryAssignment>> GetDeliveredOrdersAsync(DateTime startDate, DateTime endDate,PageRequest pageRequest);
        Task<decimal> GetTotalDeliveryFeesAsync(Guid deliveryAgentId, DateTime startDate, DateTime endDate);
        Task<int> CountPendingDeliveriesAsync(Guid deliveryAgentId);
        Task<int> CountDeliveredOrdersAsync(Guid deliveryAgentId);
    }

}
