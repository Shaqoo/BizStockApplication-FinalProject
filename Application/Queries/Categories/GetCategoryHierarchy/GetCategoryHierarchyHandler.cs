using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Categories.GetCategoryHierarchy
{
    public class GetCategoryTreeHandler(
    ICategoryRepository categoryRepository,
    IMemoryCacheService cacheService)
    : IRequestHandler<GetCategoryTreeQuery, Result<List<CategoryTreeDto>>>
    {
        private const string CacheKey = "category_tree";

        public async Task<Result<List<CategoryTreeDto>>> Handle(GetCategoryTreeQuery request, CancellationToken cancellationToken)
        {
            var result = await cacheService.GetOrAddAsync(CacheKey,
                async () =>
                {
                    var categories = await categoryRepository.GetAllCategoriesWithHierarchyAsync(cancellationToken);
                    var tree = CategoryTreeBuilder.BuildHierarchy(categories);
                    return tree;
                },
                TimeSpan.FromHours(1));  

            return Result<List<CategoryTreeDto>>.Success(result);
        }
    }

}
