using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.StockMovements.GetById
{
    public class GetStockMovementByIdHandler(IStockMovementRepository stockMovementRepository,
        IMemoryCacheService distributedCacheService) : IRequestHandler<GetStockMovementByIdQuery, Result<StockMovementDto>>
    {
        public async Task<Result<StockMovementDto>> Handle(GetStockMovementByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetStockMovementById:{request.Id}";

            var result = await distributedCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var movement = await stockMovementRepository.GetByIdAsync(request.Id);
                    return movement;
                },TimeSpan.FromMinutes(10));

            if (result != null)
                return Result<StockMovementDto>.Success(result.ToDto());

            return Result<StockMovementDto>.Failure("Stock Movement Not Found");
        }
    }
}
