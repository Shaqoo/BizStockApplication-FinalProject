using Application.Commands.Carts.DecreaseCartItemQuantity;
using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Carts.DecreaseQuantity
{
    public class DecreaseCartItemQuantityCommandHandler
        : IRequestHandler<DecreaseCartItemQuantityCommand, Result<CartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DecreaseCartItemQuantityCommandHandler> _logger;

        public DecreaseCartItemQuantityCommandHandler(
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork,
            ILogger<DecreaseCartItemQuantityCommandHandler> logger)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<CartDto>> Handle(DecreaseCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Request;

            var cart = await _cartRepository.GetByIdAsync(dto.CartId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found. CartId: {CartId}", dto.CartId);
                return Result<CartDto>.Failure("Cart not found.");
            }

            var decreased = cart.DecreaseOne(dto.ProductId);
            if (!decreased)
            {
                _logger.LogWarning("Product {ProductId} not found in Cart {CartId}", dto.ProductId, dto.CartId);
                return Result<CartDto>.Failure("Product not found in cart.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _cartRepository.UpdateAsync(cart);
                await _unitOfWork.CommitTransactionAsync();

                var total = await _cartRepository.GetTotalCountAsync(cart.Id);
                var cartDto = cart.ToDto();
                cartDto.TotalQuantity = total;

                _logger.LogInformation("Decreased quantity by 1. CartId: {CartId}, ProductId: {ProductId}", dto.CartId, dto.ProductId);
                return Result<CartDto>.Success(cartDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while decreasing quantity. CartId: {CartId}", dto.CartId);
                return Result<CartDto>.Failure("An error occurred while decreasing the item quantity.");
            }
        }
    }
}
