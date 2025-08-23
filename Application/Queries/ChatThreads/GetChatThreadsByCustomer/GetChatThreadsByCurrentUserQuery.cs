using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.ChatThreads.GetChatThreadsByCustomer
{
    public record GetChatThreadsByCurrentUserQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<ChatThreadDto>>>;

}
