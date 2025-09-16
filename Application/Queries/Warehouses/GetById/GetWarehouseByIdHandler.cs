using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Warehouses.GetById
{
    public class GetWarehouseByIdHandler(IMemoryCacheService distributedCache,
        IWarehouseRepository warehouseRepository) : IRequestHandler<GetWarehouseByIdQuery,Result<WarehouseDto>>
    {
        public async Task<Result<WarehouseDto>> Handle(GetWarehouseByIdQuery query,CancellationToken cancellationToken)
        {
            string cacheKey = $"GetWarehouseByIdQuery:Id{query.Id}";

            var result = await distributedCache.GetOrAddAsync(cacheKey,
               async () =>
               {
                   var data = await warehouseRepository.GetByIdAsync(query.Id);
                   var count = await warehouseRepository.GetCount(query.Id);
                   if(data != null)
                       return new Tuple<Warehouse,int>(data,count);
                   return null;
               },TimeSpan.FromMinutes(1));

            if (result is null)
                return Result<WarehouseDto>.Failure($"Warehouse With Id: {query.Id} Not Found");

            return Result<WarehouseDto>.Success(new WarehouseDto(result.Item1.Id, result.Item1.Name, result.Item1.Location,
                result.Item1.IsActive,result.Item2));
        }
    }
}
