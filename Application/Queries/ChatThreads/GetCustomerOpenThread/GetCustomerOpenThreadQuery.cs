using Application.Dto;
using MediatR;


namespace Application.Queries.ChatThreads.GetCustomerOpenThread
{
    public record GetCustomerOpenThreadQuery : IRequest<Result<ChatThreadDto>>;
}
