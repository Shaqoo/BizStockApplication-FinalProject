using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.ChatThreads.GetChatThreadsByAgent
{
    public record GetChatThreadsByAssignedAgentQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedList<ChatThreadDto>>>;

}
