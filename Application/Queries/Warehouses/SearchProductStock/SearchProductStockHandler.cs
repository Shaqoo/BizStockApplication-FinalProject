using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Warehouses.SearchProductStock
{
    public class SearchProductStockQueryHandler : IRequestHandler<SearchProductStockQuery, Result<PaginatedList<ProductStockSummaryDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IWarehouseRepository _warehouseRepository;

        public SearchProductStockQueryHandler(IProductRepository productRepository,IMemoryCacheService memoryCacheService ,IWarehouseRepository warehouseRepository)
        {
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<PaginatedList<ProductStockSummaryDto>>> Handle(SearchProductStockQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"SearchProductStock_{request.Keyword}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var cachedResult = await _memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var product = await _productRepository.SearchProductsAsync(request.Keyword, request.PageRequest);
                if (product == null)
                    return Result<PaginatedList<ProductStockSummaryDto>>.Failure("Product not found");

                var productStockSummaries = new List<ProductStockSummaryDto>();
                foreach (var item in product.Items)
                {
                    var stockByWarehouses = await _warehouseRepository.GetStockByProductIdAsync(item.Id);
                    var dto = new ProductStockSummaryDto(
                        item.Id,
                        item.Name,
                        stockByWarehouses.Sum(s => s.Quantity),
                        stockByWarehouses
                    );
                    productStockSummaries.Add(dto);
                }
                var paginatedResult = new PaginatedList<ProductStockSummaryDto>(
                    productStockSummaries,
                    product.TotalCount,
                    product.PageSize,
                    product.PageSize
                );
                return Result<PaginatedList<ProductStockSummaryDto>>.Success(paginatedResult);
            }, TimeSpan.FromMinutes(10));

            return cachedResult;
        }
    }

}
