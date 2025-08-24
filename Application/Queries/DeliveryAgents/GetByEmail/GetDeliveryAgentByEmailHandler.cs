using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.DeliveryAgents.GetByEmail
{
    public class GetDeliveryAgentByEmailHandler(IDeliveryAgentRepository deliveryAgentRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetDeliveryAgentEmailQuery, Result<DeliveryAgentDto>>
    {
        public async Task<Result<DeliveryAgentDto>> Handle(GetDeliveryAgentEmailQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetDeliveryAgentEmailQuery:{request.Email}";

            var deliveryAgentDto = await memoryCacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var deliveryAgent = await deliveryAgentRepository.GetByEmailAsync(request.Email);
                    if (deliveryAgent is null)
                        return null!;
                    return deliveryAgent.DeliveryAgentAsDto();
                },
                TimeSpan.FromMinutes(10)
            );
            if (deliveryAgentDto is null)
                return Result<DeliveryAgentDto>.Failure("Delivery Agent not found.");

            return Result<DeliveryAgentDto>.Success(deliveryAgentDto);
        }
    }
}
