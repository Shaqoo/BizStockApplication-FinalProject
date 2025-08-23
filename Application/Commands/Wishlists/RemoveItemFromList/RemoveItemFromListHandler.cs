using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Wishlists.RemoveItemFromList
{
    public class RemoveItemFromListHandler : IRequestHandler<RemoveItemFromListCommand, Result<string>>
    {
        private readonly IAuthService _authService;
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveItemFromListHandler> _logger;

        public RemoveItemFromListHandler(
            IAuthService authService,
            IWishlistRepository wishlistRepository,
            ILogger<RemoveItemFromListHandler> logger,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _wishlistRepository = wishlistRepository;
            _logger = logger;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(RemoveItemFromListCommand request, CancellationToken cancellationToken)
        {
            var user = _authService.CurrentUser();
            if (user == null)
            {
                _logger.LogWarning("Unauthorized attempt to remove wishlist item.");
                return Result<string>.Failure("Unauthorized access to remove from wishlist.");
            }

            var wishlist = await _wishlistRepository.GetByUserIdAsync(user.Id);
            if (wishlist == null)
            {
                _logger.LogWarning("Wishlist not found for user {UserId}", user.Id);
                return Result<string>.Failure("Wishlist not found.");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product {ProductId} not found for wishlist removal.", request.ProductId);
                return Result<string>.Failure("Product not found.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                wishlist.RemoveItem(request.ProductId);
                await _wishlistRepository.UpdateAsync(wishlist);

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Product {ProductId} removed from wishlist {WishlistId} for user {UserId}.",
                    request.ProductId, wishlist.Id, user.Id);

                return Result<string>.Success("Product removed from wishlist successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while removing product {ProductId} from wishlist {WishlistId} for user {UserId}.",
                    request.ProductId, wishlist?.Id, user.Id);

                return Result<string>.Failure("An error occurred while removing item from wishlist.");
            }
        }
    }
}
