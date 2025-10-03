using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Queries.SalesOrders.GetOrderCostAndETA
{
    public class GetOrderCostAndETAQueryHandler(
    IAuditLogRepository auditLogRepository,
    IDeliveryAddressRepository deliveryAddressRepository,
    ICustomerRepository customerRepository,
    ICartRepository cartRepository,
    IFezService fezService,
    IAuthService authService,
    IHttpContextAccessor httpContextAccessor,
    ILogger<GetOrderCostAndETAQueryHandler> logger
) : IRequestHandler<GetOrderCostAndETAQuery, Result<GetOrderCostAndETAResponseDto>>
    {
        public async Task<Result<GetOrderCostAndETAResponseDto>> Handle(GetOrderCostAndETAQuery request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser is null)
            {
                var msg = $"User not Authenticated";
                logger.LogWarning(msg);
                return Result<GetOrderCostAndETAResponseDto>.Failure(msg);
            }
            try
            {
                var deliveryAddress = await deliveryAddressRepository.GetByIdAsync(request.DeliveryAddressId);
                if (deliveryAddress is null)
                {
                    var msg = $"Delivery address not found. Id={request.DeliveryAddressId}";
                    logger.LogWarning(msg);
                    await auditLogRepository.AddAsync(new AuditLog(currentUser.Id, "GetOrderCostAndETA_Failed", "DeliveryAddress", request.DeliveryAddressId, msg,request.RequestMetadata.IpAddress,request.RequestMetadata.UserAgent));
                    return Result<GetOrderCostAndETAResponseDto>.Failure(msg);
                }
                var customer = await customerRepository.GetByIdAsync(deliveryAddress.CustomerId);
                if (customer is null)
                {
                    var msg = $"Customer not found for DeliveryAddress={request.DeliveryAddressId}";
                    logger.LogWarning(msg);
                    await auditLogRepository.AddAsync(new AuditLog(currentUser.Id, "GetOrderCostAndETA_Failed", "Customer", deliveryAddress.CustomerId, msg,request.RequestMetadata.IpAddress,request.RequestMetadata.UserAgent));
                    return Result<GetOrderCostAndETAResponseDto>.Failure(msg);
                }
                var cart = await cartRepository.GetByUserIdAsync(currentUser.Id);
                if (cart is null)
                {
                    var msg = $"Cart not found for UserId={currentUser.Id}";
                    logger.LogWarning(msg);
                    await auditLogRepository.AddAsync(new AuditLog(currentUser.Id, "GetOrderCostAndETA_Failed", "Cart", currentUser.Id, msg, request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent));
                    return Result<GetOrderCostAndETAResponseDto>.Failure(msg);
                }

                var totalWeight = cart.Items.Sum(a => a.Quantity * a.Product.Weight);

                var costEstimateDto = new CostEstimateRequestDto
                {
                    DestinationState = deliveryAddress.State.Name,
                    PickUpState = "Ogun", 
                    Weight = totalWeight
                };

                var deliveryEstimateDto = new DeliveryTimeEstimateRequestDto
                {
                    DeliveryType = "local",
                    DropOffState = deliveryAddress.State.Name,
                    PickUpState = "Ogun"
                };

                var itemsCost = await fezService.GetCostAsync(costEstimateDto);
                var deliveryEstimate = await fezService.GetDeliveryTimeEstimateAsync(deliveryEstimateDto);

                if (!itemsCost.Success || !deliveryEstimate.Success || itemsCost.Data is null || deliveryEstimate.Data is null)
                {
                    var msg = "Failed to fetch cost or delivery estimate from Fez API";
                    logger.LogWarning(msg);
                    await auditLogRepository.AddAsync(new AuditLog(currentUser.Id, "GetOrderCostAndETA_Failed", "FezService", null, msg,request.RequestMetadata.IpAddress,request.RequestMetadata.UserAgent));
                    return Result<GetOrderCostAndETAResponseDto>.Failure(msg);
                }

                httpContextAccessor.HttpContext?.Session.SetDeliveryInfo(deliveryAddress.Id,itemsCost.Data.Cost.First().Cost,deliveryEstimate.Data.ETA);

                var response = new GetOrderCostAndETAResponseDto(itemsCost.Data, deliveryEstimate.Data);

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "GetOrderCostAndETA_Success",
                    "Order",
                    request.DeliveryAddressId,
                    $"Cost={itemsCost.Data.Cost}, ETA={deliveryEstimate.Data.ETA}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                return Result<GetOrderCostAndETAResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error in GetOrderCostAndETA handler");

                await auditLogRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "GetOrderCostAndETA_Exception",
                    "Order",
                    request.DeliveryAddressId,
                    ex.Message,
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                return Result<GetOrderCostAndETAResponseDto>.Failure("An unexpected error occurred while calculating cost and ETA");
            }
        }
    }

}
