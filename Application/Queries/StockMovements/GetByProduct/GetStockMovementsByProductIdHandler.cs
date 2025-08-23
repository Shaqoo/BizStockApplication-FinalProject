using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByProduct
{
    public class GetStockMovementsByProductIdHandler(IStockMovementRepository stockMovementRepository,
        IMemoryCacheService distributedCacheService)
        : IRequestHandler<GetStockMovementByProductIdQuery, Result<PaginatedList<StockMovementDto>>>
    {
        public async Task<Result<PaginatedList<StockMovementDto>>> Handle(GetStockMovementByProductIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetStockMovementByProductIdQuery:{request.ProductId}:Page:{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var result = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var stockMovements = await stockMovementRepository.GetByProductId(request.ProductId,request.PageRequest);
                    return stockMovements;
                });

            return Result<PaginatedList<StockMovementDto>>.Success(result);
        }
    }
}
