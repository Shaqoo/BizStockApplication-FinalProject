using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Queries.Brands.NewFolder;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Brands.GetById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMemoryCacheService _cache;

        public GetBrandByIdQueryHandler(IBrandRepository brandRepository, IMemoryCacheService cache)
        {
            _brandRepository = brandRepository;
            _cache = cache;
        }

        public async Task<Result<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"brand:{request.Id}";

            var brand = await _cache.GetOrAddAsync(
                cacheKey,
                async () => await _brandRepository.GetByIdAsync(request.Id),
                TimeSpan.FromMinutes(10)
            );

            if (brand is null)
                return Result<BrandDto>.Failure("Brand not found.");

            var dto = new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                WebsiteUrl = brand.WebsiteUrl,
                LogoUrl = brand.LogoUrl,
                Description = brand.Description
            };

            return Result<BrandDto>.Success(dto);
        }
    }

}
