using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.ChatThreads.GetAllChatThreads
{
    public record GetAllChatThreadsQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedList<ChatThreadDto>>>;

}
