using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.AuditLogs.GetAllLogs
{
    public record GetAllAuditLogsQuery(PageRequest PageRequest)
    : IRequest<PaginatedList<AuditLogReadDto>>;

}
