using Application.Dto;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatMessages.GetMessagesByThread
{
    public record GetMessagesByThreadIdQuery(
    Guid ChatThreadId,
    PageRequest PageRequest) : IRequest<Result<PaginatedList<MessageDto>>>;

}
