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
    public interface ISalesOrderItemRepository : IBaseRepository<SalesOrderItem>
    {
        Task<IEnumerable<SalesOrderItem>> GetBySalesOrderIdAsync(Guid salesOrderId);
        Task<IEnumerable<SalesOrderItem>> GetByProductIdAsync(Guid productId);
        Task<decimal> GetTotalSalesForProductAsync(Guid productId);
        Task<int> GetTotalUnitsSoldForProductAsync(Guid productId);
        Task<PaginatedList<SalesOrderItem>> GetByDateRangeAsync(DateTime start, DateTime end,PageRequest pageRequest);
    }


}
