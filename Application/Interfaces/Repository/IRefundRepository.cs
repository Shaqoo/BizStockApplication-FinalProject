using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IRefundRepository
    {
        Task<Refund?> GetByIdAsync(Guid id);
        Task<IEnumerable<Refund>> GetByOrderIdAsync(Guid orderId);
        Task AddAsync(Refund refund);
        Task UpdateAsync(Refund refund);
        Task<PaginatedList<Refund>> GetAllAsync(PageRequest pageRequest);
    }
}
