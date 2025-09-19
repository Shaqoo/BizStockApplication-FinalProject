using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Queries.PurchaseOrders.GetPurchaseOrderById;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders
{
    public class GetPurchaseOrderByIdHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IMemoryCacheService memoryCacheService,
        ILogger<GetPurchaseOrderByIdHandler> logger
    ) : IRequestHandler<GetPurchaseOrderByIdQuery, Result<PurchaseOrderDetailDto>>
    {
        public async Task<Result<PurchaseOrderDetailDto>> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"PurchaseOrder:{request.PurchaseOrderId}";

                var po = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
                {
                    logger.LogInformation("Cache miss for {CacheKey}. Fetching from database...", cacheKey);

                    var purchaseOrder = await purchaseOrderRepository.GetPurchaseOrderDetailsById(request.PurchaseOrderId);
                    return purchaseOrder;
                },TimeSpan.FromMinutes(5));

                if (po == null)
                {
                    logger.LogWarning("PurchaseOrder {PurchaseOrderId} not found", request.PurchaseOrderId);
                    return Result<PurchaseOrderDetailDto>.Failure("Purchase Order not found");
                }

                logger.LogInformation("Successfully retrieved PurchaseOrder {PurchaseOrderId}", request.PurchaseOrderId);
                return Result<PurchaseOrderDetailDto>.Success(po);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving PurchaseOrder {PurchaseOrderId}", request.PurchaseOrderId);
                return Result<PurchaseOrderDetailDto>.Failure("An error occurred while retrieving purchase order");
            }
        }
    }
}
