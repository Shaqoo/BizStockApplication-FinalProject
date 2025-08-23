using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Nest;

namespace Application.Queries.Wishlists.GetWishlistByUser
{
    public class GetWishlistByUserIdHandler(IWishlistRepository wishlistRepository,
        IMemoryCacheService memoryCacheService,
        IAuthService authService) : IRequestHandler<GetWishlistByUserIdQuery, Result<WishlistDto>>
    {
        public async Task<Result<WishlistDto>> Handle(GetWishlistByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                return Result<WishlistDto>.Failure("User Not Authenticated");
            }
            string cacheKey = $"GetWishlistByUserIdQuery:{user.Id}";
            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var wishlist = await wishlistRepository.GetByUserIdAsync(user.Id);
                    if(wishlist == null)
                    {
                        return Result<WishlistDto>.Failure($"Wishlist Not Found For User {user.Id}");
                    }
                    return Result<WishlistDto>.Success(new WishlistDto(wishlist.Id,wishlist.UserId));
                });

            return result;
        }
    }
}
