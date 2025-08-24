using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAgents.GetByEmail
{
    public record GetDeliveryAgentEmailQuery(string Email) : IRequest<Result<DeliveryAgentDto>>;
}
