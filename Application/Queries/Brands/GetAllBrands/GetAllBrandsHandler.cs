using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Brands.GetAllBrands
{
    public class GetPaginatedBrandsQueryHandler : IRequestHandler<GetPaginatedBrandsQuery, Result<PaginatedList<BrandDto>>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMemoryCacheService _cache;

        public GetPaginatedBrandsQueryHandler(IBrandRepository brandRepository,IMemoryCacheService cacheService)
        {
            _brandRepository = brandRepository;
            _cache = cacheService;
        }

        public async Task<Result<PaginatedList<BrandDto>>> Handle(GetPaginatedBrandsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"brands:page:{request.PageRequest.Page}:{request.PageRequest.PageSize}";


            var brands = await _cache.GetOrAddAsync(
                cacheKey,
                async () => await _brandRepository.GetAllAsync(request.PageRequest),
                TimeSpan.FromMinutes(5) 
            );


            var result = brands.Items.Select(b => new BrandDto
            {
                Id = b.Id,
                Name = b.Name,
                WebsiteUrl = b.WebsiteUrl,
                LogoUrl = b.LogoUrl,
                Description = b.Description
            }).ToList();

            return Result<PaginatedList<BrandDto>>.Success(new PaginatedList<BrandDto>(result,brands.TotalCount,brands.PageNumber,brands.PageSize));
        }
    }

}
