using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Wishlists.GetItemsByUser
{
    public class GetWishlistItemsByUserHandler(IWishlistRepository wishlistRepository,
        IMemoryCacheService memoryCacheService,
        IAuthService authService) : IRequestHandler<GetWishlistItemsByUserQuery, Result<PaginatedList<WishlistItemDto>>>
    {
        public async Task<Result<PaginatedList<WishlistItemDto>>> Handle(GetWishlistItemsByUserQuery request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                return Result<PaginatedList<WishlistItemDto>>.Failure("User Not Found");
            }
            var cacheKey = $"GetWishlistItemsByUserQuery:Page:{request.PageRequest.Page}:PageSize:{request.PageRequest.PageSize}";
            var result = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var items = await wishlistRepository.GetAllByUserAsync(request.PageRequest, user.Id);
                    return items;
                },TimeSpan.FromMinutes(10));

            return Result<PaginatedList<WishlistItemDto>>.Success(result);
        }
    }
}
