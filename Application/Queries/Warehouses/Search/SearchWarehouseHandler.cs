using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Warehouses.Search
{
    public class SearchWarehouseHandler(IWarehouseRepository warehouseRepository,
        IMemoryCacheService distributedCache) : IRequestHandler<SearchWarehouseQuery, Result<PaginatedList<WarehouseDto>>>
    {
        public async Task<Result<PaginatedList<WarehouseDto>>> Handle(SearchWarehouseQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"SearchWarehouseQuery:Keyword{request.Keyword}:PageSize{request.PageRequest.PageSize}:PageNumber{request.PageRequest.Page}";
            var result = await distributedCache.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var data = await warehouseRepository.SearchWarehousesAsync(request.Keyword, request.PageRequest);
                    return data;
                },TimeSpan.FromMinutes(20));
                return Result<PaginatedList<WarehouseDto>>.Success(result);
        }
    }
}
