using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Tags.GetAllTags
{
    public class GetAllTagsPaginatedQueryHandler
    : IRequestHandler<GetAllTagsPaginatedQuery, Result<PaginatedList<TagDto>>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<GetAllTagsPaginatedQueryHandler> _logger;
        private readonly IMemoryCacheService _distributedCacheService;

        public GetAllTagsPaginatedQueryHandler(
            ITagRepository tagRepository,
            ILogger<GetAllTagsPaginatedQueryHandler> logger,
            IMemoryCacheService cacheService)
        {
            _tagRepository = tagRepository;
            _logger = logger;
            _distributedCacheService = cacheService;
        }

        public async Task<Result<PaginatedList<TagDto>>> Handle(
            GetAllTagsPaginatedQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching paginated list of tags. Page: {Page}, Size: {PageSize}",
                request.PageRequest.Page, request.PageRequest.PageSize);

            string cacheKey = $"GetAllTagsPaginated_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var cachedResult = await _distributedCacheService.GetOrAddAsync<PaginatedList<TagDto>>(cacheKey,
                async () =>
                {
                    var paginated = await _tagRepository.GetAllAsync(request.PageRequest);
                    _logger.LogInformation("Successfully fetched {Count} tags.", paginated.Items.Count);
                    return paginated;
                },TimeSpan.FromMinutes(10));

            return Result<PaginatedList<TagDto>>.Success(cachedResult);
        }
    }

}
