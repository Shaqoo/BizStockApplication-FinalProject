using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByMovementType
{
    public class GetStockMovementsByMovementTypeHandler(IStockMovementRepository stockMovementRepository,
        IMemoryCacheService distributedCacheService)
        : IRequestHandler<GetStockMovementsByMovementTypeQuery, Result<PaginatedList<StockMovementDto>>>
    {
        public async Task<Result<PaginatedList<StockMovementDto>>> Handle(GetStockMovementsByMovementTypeQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetStockMovementByMovementTypeQuery:{request.MovementType.ToString()}:Page:{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var result = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var stockMovements = await stockMovementRepository.GetByMovementType(request.MovementType, request.PageRequest);
                    return stockMovements;
                });

            return Result<PaginatedList<StockMovementDto>>.Success(result);
        }
    }

}
