using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Tags.GetByProductId
{
    public class GetTagsByProductIdHandler(IMemoryCacheService distributedCache,
        ITagRepository tagRepository) : IRequestHandler<GetTagsByProductIdQuery, Result<PaginatedList<TagDto>>>
    {
        public async Task<Result<PaginatedList<TagDto>>> Handle(GetTagsByProductIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetTagsByProductId-{request.ProductId}-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var cachedResult = await distributedCache.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var tags = await tagRepository.GetByProductIdAsync(request.ProductId, request.PageRequest);
                    return Result<PaginatedList<TagDto>>.Success(tags);
                },TimeSpan.FromMinutes(10));

            return cachedResult;
        }
    }
}
