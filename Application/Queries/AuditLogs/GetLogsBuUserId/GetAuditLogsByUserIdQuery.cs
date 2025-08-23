using Application.Dto;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AuditLogs.GetLogsBuUserId
{
    public record GetAuditLogsByUserIdQuery(Guid UserId, PageRequest PageRequest)
    : IRequest<PaginatedList<AuditLogReadDto>>;

}
