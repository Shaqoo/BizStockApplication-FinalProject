using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class LostAccessRequestRepository : ILostAccessRequestRepository
    {
        private readonly BizStockContext _context;

        public LostAccessRequestRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<LostAccessRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.LostAccessRequests
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<PaginatedList<LostAccessRequest>> GetPendingRequestsAsync(
            PageRequest pageRequest,
            CancellationToken cancellationToken = default)
        {
            var query = _context.LostAccessRequests
                .Where(r => r.Status == LostAccessStatus.Pending)
                .OrderBy(r => r.SubmittedAt);

            var count = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<LostAccessRequest>(items, count, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task AddAsync(LostAccessRequest request, CancellationToken cancellationToken = default)
        {
            await _context.LostAccessRequests.AddAsync(request, cancellationToken);
        }

        public async Task UpdateAsync(LostAccessRequest request, CancellationToken cancellationToken = default)
        {
            _context.LostAccessRequests.Update(request);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<LostAccessRequest>> SearchByEmailAsync(
    string email,
    PageRequest pageRequest,
    CancellationToken cancellationToken = default)
        {
            var query = _context.LostAccessRequests
                .Where(r => r.UserIdentifier.Contains(email))
                .OrderByDescending(r => r.SubmittedAt);

            var count = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<LostAccessRequest>(items, count, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<LostAccessRequest?> SearchByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.LostAccessRequests
                .FirstOrDefaultAsync(r => r.UserIdentifier == email && r.Status == LostAccessStatus.Pending, cancellationToken);
        }
    }
}
