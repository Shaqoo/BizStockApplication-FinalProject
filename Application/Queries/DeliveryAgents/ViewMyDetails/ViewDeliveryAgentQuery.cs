using Application.Dto;
using MediatR;

namespace Application.Queries.DeliveryAgents.ViewMyDetails
{
    public record ViewDeliveryAgentQuery : IRequest<Result<DeliveryAgentDto>>;
    

}
