using global::Application.Dto;
using global::Application.Interfaces.Repository;
using global::Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Carts.RemoveItem
{
    public class RemoveCartItemCommandHandler
        : IRequestHandler<RemoveCartItemCommand, Result<string>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveCartItemCommandHandler> _logger;

        public RemoveCartItemCommandHandler(
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveCartItemCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var removeRequest = request.RemoveCartItemRequest;

            _logger.LogInformation("Removing item {ItemId} from Cart {CartId}",
                removeRequest.ProductId, removeRequest.CartId);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var cart = await _cartRepository.GetByIdAsync(removeRequest.CartId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found. Id: {CartId}", removeRequest.CartId);
                    return Result<string>.Failure("Cart not found.");
                }

                bool removed = cart.RemoveItem(removeRequest.ProductId);
                if (!removed)
                {
                    _logger.LogWarning("Item not found in cart. CartId: {CartId}, ItemId: {ItemId}",
                        removeRequest.CartId, removeRequest.ProductId);
                    return Result<string>.Failure("Item not found in cart.");
                }

                await _cartRepository.UpdateAsync(cart);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Item {ItemId} removed successfully from Cart {CartId}",
                    removeRequest.ProductId, removeRequest.CartId);

                return Result<string>.Success("Item removed successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while removing item {ItemId} from Cart {CartId}",
                    removeRequest.ProductId, removeRequest.CartId);
                return Result<string>.Failure("An error occurred while removing item from cart.");
            }
        }
    }
}
