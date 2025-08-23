using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Domain.Entities;
using MediatR;

namespace Application.Queries.AuditLogs.GetAllLogs
{
    public class GetAllAuditLogsQueryHandler
    : IRequestHandler<GetAllAuditLogsQuery, PaginatedList<AuditLogReadDto>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMemoryCacheService _cache;

        public GetAllAuditLogsQueryHandler(IAuditLogRepository auditLogRepository, IMemoryCacheService cache)
        {
            _auditLogRepository = auditLogRepository;
            _cache = cache;
        }

        public async Task<PaginatedList<AuditLogReadDto>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"auditlogs_all_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var result = await _cache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var logs = await _auditLogRepository.GetAllAsync(request.PageRequest);

                    return new PaginatedList<AuditLogReadDto>(
                        logs.Items.Select(a => a.MapToDto()).ToList(),
                        logs.TotalCount,
                        logs.PageNumber,
                        logs.PageSize
                    );
                },
                TimeSpan.FromMinutes(10)
            );
            return result;
        }
    }

}
