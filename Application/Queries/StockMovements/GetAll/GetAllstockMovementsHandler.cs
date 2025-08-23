using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System.Collections.Immutable;

namespace Application.Queries.StockMovements.GetAll
{
    public class GetAllstockMovementsHandler(IMemoryCacheService distributedCacheService,
        IStockMovementRepository stockMovementRepository) : IRequestHandler<GetAllStockMovementsQuery, Result<PaginatedList<StockMovementDto>>>
    {
        public async Task<Result<PaginatedList<StockMovementDto>>> Handle(GetAllStockMovementsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetAllStockMovements:PageSize:{request.PageRequest.PageSize}:Page:{request.PageRequest.Page}";

            var result = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var result = await stockMovementRepository.GetAllAsync(pageRequest: request.PageRequest);
                    return result;
                });

            return Result<PaginatedList<StockMovementDto>>.Success(new PaginatedList<StockMovementDto>
            {
                Items = result.Items.Select(a => a.ToDto()).ToImmutableList(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
            });
        }
    }
}
