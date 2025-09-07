using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Wishlists.CheckIfIsInWishlist
{
    public class CheckIfIsInWishlistHandler(IWishlistRepository wishlistRepository,
        IAuthService authService) : IRequestHandler<CheckIfIsInWishlistQuery, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CheckIfIsInWishlistQuery request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                return Result<bool>.Failure("User Not Found");
            }

            var exists = await wishlistRepository.CheckIfItemExists(user.Id, request.ProductId);
            return Result<bool>.Success(exists);
        }
    }
}
