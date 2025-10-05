using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Queries.SalesOrders.TrackItem
{
    public record TrackItemQuery(string trackingNumber) : IRequest<Result<TrackOrderResponseDto>>;

}
