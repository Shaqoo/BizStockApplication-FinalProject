using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.StockMovements.GetByWarehouse
{
    public class GetStockMovementsByWarehouseIdHandler(IMemoryCacheService cacheService,
        IStockMovementRepository stockMovementRepository) : IRequestHandler<GetStockMovementsByWarehouseIdQuery,Result<PaginatedList<StockMovementDto>>>
    {
        public async Task<Result<PaginatedList<StockMovementDto>>> Handle(GetStockMovementsByWarehouseIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetStockMovementsByWarehouseId{request.WarehouseId}";

            var result = await cacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var stockMovements = await stockMovementRepository.GetByWarehousePagedAsync(request.WarehouseId,request.PageRequest);
                    return stockMovements;
                });

            return Result<PaginatedList<StockMovementDto>>.Success(result);
        }
    }
}
