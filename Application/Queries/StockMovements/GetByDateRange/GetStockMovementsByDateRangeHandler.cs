using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByDateRange
{
    public class GetStockMovementsByDateRangeHandler(IMemoryCacheService distributedCacheService,
        IStockMovementRepository stockMovementRepository)
        : IRequestHandler<GetStockMovementsByDateRangeQuery, Result<PaginatedList<StockMovementDto>>>
    {
        public async Task<Result<PaginatedList<StockMovementDto>>> Handle(GetStockMovementsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetStockMovementsByDateRange:StartDate:{request.StartDate}:EndDate:{request.EndDate}:Page{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";

            var result = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var stockMovements = await stockMovementRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, request.PageRequest);
                    return stockMovements;
                });

            return Result<PaginatedList<StockMovementDto>>.Success(result);
        }
    }
}
