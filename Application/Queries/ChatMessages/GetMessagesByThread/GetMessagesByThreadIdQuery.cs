using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.ChatMessages.GetMessagesByThread
{
    public record GetMessagesByThreadIdQuery(
    Guid ChatThreadId,
    PageRequest PageRequest) : IRequest<Result<PaginatedList<MessageDto>>>;

}
