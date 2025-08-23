using Application.Dto;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AuditLogs.SearchLogs
{
    public record SearchAuditLogsQuery(string Keyword, PageRequest PageRequest)
    : IRequest<PaginatedList<AuditLogReadDto>>;

}
