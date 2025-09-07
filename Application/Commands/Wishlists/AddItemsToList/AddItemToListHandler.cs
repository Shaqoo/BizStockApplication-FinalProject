using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Wishlists.AddItemsToList
{
    public class AddItemToListHandler(
        IAuthService authService,
        IWishlistRepository wishlistRepository,
        ILogger<AddItemToListHandler> logger,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<AddItemToListCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddItemToListCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                logger.LogWarning("Unauthorized access attempt to AddItemsToListCommand");
                return Result<string>.Failure("Unauthorized");
            }

            logger.LogInformation("Handling AddItemsToListCommand for UserId: {UserId}, ProductId: {ProductId}",
                user.Id, request.ProductId);

            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                logger.LogWarning("Product not found: {ProductId}", request.ProductId);
                return Result<string>.Failure("Product not found");
            }

            var wishlist = await wishlistRepository.GetByUserIdAsync(user.Id);
            if (wishlist == null)
            {
                logger.LogWarning("Wishlist not found for UserId: {UserId}", user.Id);
                return Result<string>.Failure("Wishlist not found");
            }

            if (wishlist.Items.Any(i => i.ProductId == product.Id))
            {
                logger.LogInformation("Product {ProductId} already exists in wishlist for UserId: {UserId}",
                    product.Id, user.Id);
                return Result<string>.Failure("Product already exists in wishlist");
            }

            try
            {
                await unitOfWork.BeginTransactionAsync();

                 
                await wishlistRepository.AddItemsAsync(new WishlistItem(wishlist.Id,product.Id));

                await unitOfWork.CommitTransactionAsync();

                logger.LogInformation("Product {ProductId} added to wishlist for UserId: {UserId}",
                    product.Id, user.Id);

                return Result<string>.Success("Product added to wishlist");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error occurred while adding product {ProductId} to wishlist for UserId: {UserId}",
                    product.Id, user.Id);
                return Result<string>.Failure("An error occurred while adding product to wishlist");
            }
        }
    }
}
