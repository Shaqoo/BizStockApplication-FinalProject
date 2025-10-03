using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Queries.SalesOrders.GetOrderCostAndETA
{
    public record GetOrderCostAndETAQuery(
    Guid DeliveryAddressId,
    RequestMetadata RequestMetadata
) : IRequest<Result<GetOrderCostAndETAResponseDto>>;

}
