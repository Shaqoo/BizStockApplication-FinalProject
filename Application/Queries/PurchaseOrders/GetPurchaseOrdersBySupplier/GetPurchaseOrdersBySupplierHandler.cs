using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.PurchaseOrders.GetPurchaseOrdersBySupplier
{
    public class GetPurchaseOrdersBySupplierHandler
        : IRequestHandler<GetPurchaseOrdersBySupplierQuery, PaginatedList<PurchaseOrderListDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ILogger<GetPurchaseOrdersBySupplierHandler> _logger;

        public GetPurchaseOrdersBySupplierHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IMemoryCacheService memoryCacheService,
            ILogger<GetPurchaseOrdersBySupplierHandler> logger)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _memoryCacheService = memoryCacheService;
            _logger = logger;
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> Handle(GetPurchaseOrdersBySupplierQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching purchase orders for SupplierId {SupplierId} | Page: {Page}, PageSize: {PageSize}",
                    request.SupplierId, request.PageRequest.Page, request.PageRequest.PageSize);

                var cacheKey = $"purchase_orders_supplier_{request.SupplierId}_page_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

                var pagedResult =  await _memoryCacheService.GetOrAddAsync(
                    cacheKey,
                    async () =>
                    {
                        var result = await _purchaseOrderRepository.GetBySupplierIdAsync(request.SupplierId, request.PageRequest);

                        _logger.LogInformation(
                            "Fetched {Count} purchase orders for SupplierId {SupplierId} (Page {Page})",
                            result.Items.Count, request.SupplierId, request.PageRequest.Page);

                        return result;
                    },
                    TimeSpan.FromMinutes(5));
                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while fetching purchase orders for SupplierId {SupplierId} | Page: {Page}, PageSize: {PageSize}",
                    request.SupplierId, request.PageRequest.Page, request.PageRequest.PageSize);

                throw; 
            }
        }
    }
}
