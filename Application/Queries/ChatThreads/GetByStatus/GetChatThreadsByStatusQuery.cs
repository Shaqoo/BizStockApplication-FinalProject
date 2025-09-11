using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;

namespace Application.Queries.ChatThreads.GetByStatus
{
    public record GetChatThreadsByStatusQuery(ChatStatus Status, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<ChatThreadDto>>>;

}
