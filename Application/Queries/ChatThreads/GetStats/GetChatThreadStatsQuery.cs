using Application.Dto;
using MediatR;

namespace Application.Queries.ChatThreads.GetStats
{
    public record GetChatThreadStatsQuery : IRequest<Result<ChatThreadStatsDto>>;

}
