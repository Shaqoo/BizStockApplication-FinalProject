using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.DeliveryAgents.ViewMyDetails
{
    public class ViewDeliveryAgentHandler(IAuthService authService,
        IUserRepository userRepository,
        IDeliveryAgentRepository deliveryAgentRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<ViewDeliveryAgentQuery, Result<DeliveryAgentDto>>
    {
        public async Task<Result<DeliveryAgentDto>> Handle(ViewDeliveryAgentQuery request, CancellationToken cancellationToken)
        {
             var currentUser = authService.CurrentUser();

             if(currentUser is null)
                return Result<DeliveryAgentDto>.Failure("User not found.");

             var checkIfExists = await userRepository.CheckIfExists(x => x.Id == currentUser.Id && !x.IsDeleted);
             if (!checkIfExists)
                 return Result<DeliveryAgentDto>.Failure("User not found.");

             var cacheKey = $"DeliveryAgentDetails:{currentUser.Id}";

                var deliveryAgentDto = await distributedCacheService.GetOrAddAsync(
                    cacheKey,
                    async () =>
                    {
                        var deliveryAgent = await deliveryAgentRepository.GetByEmailAsync(currentUser.Email);
                        if (deliveryAgent is null)
                            return null!;
                        return deliveryAgent.DeliveryAgentAsDto();
                    },
                    TimeSpan.FromMinutes(10)  
                );
            if(deliveryAgentDto is null )
                return Result<DeliveryAgentDto>.Failure("Delivery Agent not found.");

            return Result<DeliveryAgentDto>.Success(deliveryAgentDto);
        }
    }
}
