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
    public interface ISalesOrderRepository : IBaseRepository<SalesOrder>
    {
        Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<PaginatedList<SalesOrder>> GetByCustomerIdAsync(Guid customerId,PageRequest pageRequest);
        Task<PaginatedList<SalesOrder>> GetByStatusAsync(OrderStatus status,PageRequest pageRequest);
        Task<SalesOrder?> GetWithItemsAsync(Guid salesOrderId);
        Task<SalesOrder?> GetWithDeliveryAssignmentAsync(Guid salesOrderId);
        Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
        Task<int> CountByStatusAsync(OrderStatus status);
        Task UpdateStatusAsync(SalesOrder salesOrder);
    }

}
