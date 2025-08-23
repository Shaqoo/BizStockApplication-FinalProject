using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Brands.Search
{
    public class SearchBrandsQueryHandler : IRequestHandler<SearchBrandsQuery, Result<PaginatedList<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMemoryCacheService cacheService;

        public SearchBrandsQueryHandler(IBrandRepository brandRepository,IMemoryCacheService cacheService)
        {
            _brandRepository = brandRepository;
            this.cacheService = cacheService;
        }

        public async Task<Result<PaginatedList<BrandDto>>> Handle(SearchBrandsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"SearchBrands_{request.Keyword}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";


            var brands = await cacheService.GetOrAddAsync(cacheKey, async () =>
            {
                return await _brandRepository.SearchAsync(request.Keyword, request.PageRequest);
            }, TimeSpan.FromMinutes(5));

            var result = brands.Items.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                WebsiteUrl = b.WebsiteUrl,
                LogoUrl = b.LogoUrl,
                Description = b.Description
            }).ToList();

            return Result<PaginatedList<BrandDto>>.Success(new PaginatedList<BrandDto>(result, brands.TotalCount, brands.PageNumber, brands.PageSize));
        }
    }

}
