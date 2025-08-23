using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.AuditLogs.GetLogsByAction
{
    public record GetAuditLogsByActionQuery(string Action, PageRequest PageRequest)
    : IRequest<PaginatedList<AuditLogReadDto>>;

}
