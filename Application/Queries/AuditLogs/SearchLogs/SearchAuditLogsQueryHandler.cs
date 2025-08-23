using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Application.Extensions;
using MediatR;

namespace Application.Queries.AuditLogs.SearchLogs
{
    public class SearchAuditLogsQueryHandler
    : IRequestHandler<SearchAuditLogsQuery, PaginatedList<AuditLogReadDto>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMemoryCacheService _cache;

        public SearchAuditLogsQueryHandler(IAuditLogRepository auditLogRepository, IMemoryCacheService cache)
        {
            _auditLogRepository = auditLogRepository;
            _cache = cache;
        }

        public async Task<PaginatedList<AuditLogReadDto>> Handle(SearchAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"auditlogs_search_{request.Keyword}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var result = await _cache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var logs = await _auditLogRepository.SearchAsync(request.Keyword, request.PageRequest);

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
