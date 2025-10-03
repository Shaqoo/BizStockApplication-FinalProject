using Application.Dto;
using Application.Dto.RequestModels;

namespace Application.Interfaces.Service
{
    public interface IFezService
    {
        Task<FezResponse<CreateFezOrderResponseDto>> CreateOrderAsync(List<CreateFezOrderRequestItem> requestItems);
        Task<FezResponse<CostEstimateResponseDto>> GetCostAsync(CostEstimateRequestDto request);
        Task<FezResponse<CheckOrderStatusResponseDto>> GetOrderStatusAsync(CheckOrderStatusRequestDto request);
        Task<FezResponse<IEnumerable<CheckOrderStatusResponseDto>>> GetAllOrdersAsync();
        Task<FezResponse<bool>> CancelOrderAsync(string orderNumber);
        Task<FezResponse<DeliveryTimeEstimateResponseDto>> GetDeliveryTimeEstimateAsync(DeliveryTimeEstimateRequestDto request);
        Task<FezResponse<TrackOrderResponseDto>> TrackOrderAsync(string orderNumber);
    }
}
