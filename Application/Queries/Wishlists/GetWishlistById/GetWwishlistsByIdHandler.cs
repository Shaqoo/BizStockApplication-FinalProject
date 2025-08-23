using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Wishlists.GetWishlistById
{
    public class GetWwishlistsByIdHandler(IMemoryCacheService memoryCacheService,
        IWishlistRepository wishlistRepository) : IRequestHandler<GetWishlistByIdQuery, Result<WishlistDto>>
    {
        public async Task<Result<WishlistDto>> Handle(GetWishlistByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetWishlistByIdQuery:{request.Id}";

            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var response = await wishlistRepository.GetByIdAsync(request.Id);
                    if (response == null)
                    {
                        return Result<WishlistDto>.Failure("Wishlist Not Found");
                    }
                    return Result<WishlistDto>.Success(new WishlistDto(response.Id, response.UserId));
                },TimeSpan.FromMinutes(10));

            return result;
        }
    }
}
