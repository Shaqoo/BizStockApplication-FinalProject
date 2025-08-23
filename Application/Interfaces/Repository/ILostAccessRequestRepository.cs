using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ILostAccessRequestRepository
    {
        Task<LostAccessRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedList<LostAccessRequest>> GetPendingRequestsAsync(PageRequest pageRequest,CancellationToken cancellationToken = default);
        Task AddAsync(LostAccessRequest request, CancellationToken cancellationToken = default);
        Task UpdateAsync(LostAccessRequest request, CancellationToken cancellationToken = default);
        Task<PaginatedList<LostAccessRequest>> SearchByEmailAsync(string email, PageRequest pageRequest, CancellationToken cancellationToken = default);
        Task<LostAccessRequest?> SearchByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
