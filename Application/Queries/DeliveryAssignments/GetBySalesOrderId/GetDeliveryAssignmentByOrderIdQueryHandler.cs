using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.DeliveryAssignments.GetBySalesOrderId
{
    public class GetDeliveryAssignmentByOrderIdQueryHandler(IMemoryCacheService memoryCacheService,
        IDeliveryAssignmentRepository deliveryAssignmentRepository) : IRequestHandler<GetDeliveryAssignmentByOrderIdQuery, Result<DeliveryAssignmentDto>>
    {
        public async Task<Result<DeliveryAssignmentDto>> Handle(GetDeliveryAssignmentByOrderIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetDeliveryAssignmentByOrderIdQuery{request.orderId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var assignment = await deliveryAssignmentRepository.GetBySalesOrderIdAsync(request.orderId);
                if (assignment == null)
                {
                    return Result<DeliveryAssignmentDto>.Failure("Delivery Not Found");
                }
                var dto = new DeliveryAssignmentDto
                {
                    RecipientPhone = assignment.RecipientPhone,
                    DeliveredAt = assignment.DeliveredAt,
                    DeliveryAddressId = assignment.DeliveryAddressId,
                    DeliveryAgentId = assignment.DeliveryAgentId,
                    DeliveryAgentName = assignment.DeliveryAgent?.FullName ?? "",
                    DeliveryAgentPhone = assignment.DeliveryAgent?.ContactNumber ?? "",
                    DeliveryFee = assignment.DeliveryFee,
                    ExternalDeliveryService = assignment.ExternalDeliveryService,
                    ExternalJobId = assignment.ExternalJobId,
                    Id = assignment.Id,
                    IsExternal = assignment.IsExternal,
                    Note = assignment.Note,
                    RecipientEmail = assignment.RecipientEmail,
                    RecipientName = assignment.RecipientName,
                    Status = assignment.Status,
                    SalesOrderId = assignment.SalesOrderId
                };

                return Result<DeliveryAssignmentDto>.Success(dto);
            }, TimeSpan.FromMinutes(20));

            return cachedResult ?? new Result<DeliveryAssignmentDto>();

        }
    }
}
