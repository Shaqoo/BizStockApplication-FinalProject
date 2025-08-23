using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatThreads.GetByStatus
{
    public record GetChatThreadsByStatusQuery(ChatStatus Status, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<ChatThreadDto>>>;

}
