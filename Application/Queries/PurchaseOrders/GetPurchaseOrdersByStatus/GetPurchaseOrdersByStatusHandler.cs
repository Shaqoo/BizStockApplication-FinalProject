using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersByStatus
{
    public class GetPurchaseOrdersByStatusHandler : IRequestHandler<GetPurchaseOrdersByStatusQuery, PaginatedList<PurchaseOrderListDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ILogger<GetPurchaseOrdersByStatusHandler> _logger;

        public GetPurchaseOrdersByStatusHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IMemoryCacheService memoryCacheService,
            ILogger<GetPurchaseOrdersByStatusHandler> logger)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _memoryCacheService = memoryCacheService;
            _logger = logger;
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> Handle(GetPurchaseOrdersByStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching purchase orders with status {Status} | Page: {Page}, PageSize: {PageSize}",
                    request.Status, request.PageRequest.Page, request.PageRequest.PageSize);

                var cacheKey = $"purchase_orders_status_{request.Status}_page_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

                var pagedResult =  await _memoryCacheService.GetOrAddAsync(
                    cacheKey,
                    async () =>
                    {
                        var result = await _purchaseOrderRepository
                            .FilterPurchaseOrderWithStatusPagedAsync(request.Status, request.PageRequest);

                        _logger.LogInformation(
                            "Fetched {Count} purchase orders with status {Status} for page {Page}",
                            result.Items.Count, request.Status, request.PageRequest.Page);

                        return result;
                    },
                    TimeSpan.FromMinutes(5));
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while fetching purchase orders with status {Status} | Page: {Page}, PageSize: {PageSize}",
                    request.Status, request.PageRequest.Page, request.PageRequest.PageSize);

                throw; 
            }
        }
    }
}
