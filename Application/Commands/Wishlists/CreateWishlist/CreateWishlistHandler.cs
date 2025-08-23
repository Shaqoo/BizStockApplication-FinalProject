using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Wishlists.CreateWishlist
{
    public class CreateWishlistHandler(IUserRepository userRepository,
        IWishlistRepository wishlistRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateWishlistHandler> logger) : IRequestHandler<CreateWishlistCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateWishlistCommand request, CancellationToken cancellationToken)
        {
             logger.LogInformation("Creating wishlist for user {UserId}", request.UserId);
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                logger.LogWarning("User {UserId} not found", request.UserId);
                return Result<string>.Failure("User not found");
            }   
            var wishlist = await wishlistRepository.GetByUserIdAsync(request.UserId);
            if (wishlist != null)
            {
                logger.LogWarning("Wishlist already exists for user {UserId}", request.UserId);
                return Result<string>.Failure("Wishlist already exists for this user");
            }

            await unitOfWork.BeginTransactionAsync();
            wishlist = new Wishlist(request.UserId);
            await wishlistRepository.AddAsync(wishlist);
            await unitOfWork.CommitTransactionAsync();

            logger.LogInformation("Wishlist created successfully for user {UserId}", request.UserId);
            return Result<string>.Success("Wishlist created successfully");
        }
    }
}
