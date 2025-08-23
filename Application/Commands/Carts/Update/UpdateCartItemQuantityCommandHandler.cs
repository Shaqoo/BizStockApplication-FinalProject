using Application.Commands.Carts.Update;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Carts.UpdateItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler
        : IRequestHandler<UpdateCartItemQuantityCommand, Result<string>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCartItemQuantityCommandHandler> _logger;

        public UpdateCartItemQuantityCommandHandler(
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateCartItemQuantityCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var updateRequest = request.UpdateCartItem;

            _logger.LogInformation(
                "Updating quantity for CartItem {CartItemId} in Cart {CartId} to {Quantity}",
                updateRequest.ProductId, updateRequest.CartId, updateRequest.Quantity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var cart = await _cartRepository.GetByIdAsync(updateRequest.CartId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found. Id: {CartId}", updateRequest.CartId);
                    return Result<string>.Failure("Cart not found.");
                }

                var updated = cart.UpdateItemQuantity(updateRequest.ProductId, updateRequest.Quantity);
                if (!updated)
                {
                    _logger.LogWarning("CartItem not found or invalid. CartId: {CartId}, ItemId: {ItemId}",
                        updateRequest.CartId, updateRequest.ProductId);
                    return Result<string>.Failure("Cart item not found.");
                }

                await _cartRepository.UpdateAsync(cart);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("CartItem {CartItemId} quantity updated successfully in Cart {CartId}",
                    updateRequest.ProductId, updateRequest.CartId);

                return Result<string>.Success("Cart item quantity updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating CartItem {CartItemId} in Cart {CartId}",
                    updateRequest.ProductId, updateRequest.CartId);
                return Result<string>.Failure("An error occurred while updating cart item quantity.");
            }
        }
    }
}
