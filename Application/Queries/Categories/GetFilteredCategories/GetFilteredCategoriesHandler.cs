using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Categories.GetFilteredCategories
{
    public class GetFilteredCategoriesHandler(
    ICategoryRepository categoryRepository,
    IMemoryCacheService cacheService)
    : IRequestHandler<GetFilteredCategoriesQuery, Result<PaginatedList<CategoryDto>>>
    {
        public async Task<Result<PaginatedList<CategoryDto>>> Handle(GetFilteredCategoriesQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            var cacheKey = $"categories:depth={filter.Depth}:search={filter.SearchTerm?.ToLower() ?? "null"}:page={filter.PageNumber}:size={filter.PageSize}";

            var result = await cacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    return await categoryRepository.GetFilteredCategoriesAsync(filter, cancellationToken);
                },
                TimeSpan.FromMinutes(5)  
            );

            return Result<PaginatedList<CategoryDto>>.Success(result);
        }
    }


}
