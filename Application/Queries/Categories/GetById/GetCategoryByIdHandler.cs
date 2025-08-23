using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Categories.GetById
{
    public class GetCategoryByIdHandler(ICategoryRepository categoryRepository,
        IMemoryCacheService cacheService) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"category:{request.id}";
            var cached = await cacheService.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var category = await categoryRepository.GetCategoryByIdAsync(request.id);
                    if (category == null)
                        return null!;
                    return category;
                },
                TimeSpan.FromMinutes(30) 
            );
            if (cached is null)
            return Result<CategoryDto>.Failure("Category not found or deleted.");

            return Result<CategoryDto>.Success(cached);
        }
    }

}
