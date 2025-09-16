using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Warehouses.GetAll
{
    public class GetAllWarehousesHandler(IMemoryCacheService distributedCache,
        IWarehouseRepository warehouseRepository) : IRequestHandler<GetAllWarehousesQuery, Result<PaginatedList<WarehouseDto>>>
    {
        public async Task<Result<PaginatedList<WarehouseDto>>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetAllWarehousesQuery:PageSize{request.PageRequest.PageSize}:PageNumber{request.PageRequest.Page}";
            var result = await distributedCache.GetOrAddAsync(cacheKey,
              async () =>
              {
                  var data = await warehouseRepository.GetAllAsyncWithDto(request.PageRequest);
                  return data;
              },TimeSpan.FromMinutes(1));

            return Result<PaginatedList<WarehouseDto>>.Success(result);
        }
    }
}
