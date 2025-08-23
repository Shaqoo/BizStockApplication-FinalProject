using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Carts.AddItem
{
    public class AddCartItemCommandHandler
        : IRequestHandler<AddCartItemCommand, Result<CartItemDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCartItemCommandHandler> _logger;
        private readonly IProductRepository _productRepository;

        public AddCartItemCommandHandler(
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddCartItemCommandHandler> logger,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Result<CartItemDto>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var itemRequest = request.CartItemRequest;

            if (!itemRequest.UserId.HasValue && string.IsNullOrEmpty(itemRequest.CartSessionId))
            {
                _logger.LogWarning("Invalid request: both UserId and CartSessionId are missing.");
                return Result<CartItemDto>.Failure("Invalid cart request.");
            }

            
            var product = await _productRepository.GetByIdAsync(itemRequest.ProductId);

            if (product == null)
            {
                _logger.LogWarning("Product not found with Id: {ProductId}", itemRequest.ProductId);
                return Result<CartItemDto>.Failure("Product not found.");
            }

            Cart? cart;

            if (itemRequest.UserId.HasValue && itemRequest.UserId != Guid.Empty)
            {
                cart = await _cartRepository.GetByUserIdAsync(itemRequest.UserId.Value);

                if (cart == null)
                {
                    cart = new Cart(itemRequest.UserId.Value);
                    await _cartRepository.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else if (!string.IsNullOrEmpty(itemRequest.CartSessionId))
            {
                cart = await _cartRepository.GetBySessionIdAsync(itemRequest.CartSessionId);

                if (cart == null)
                {
                    cart = new Cart(itemRequest.CartSessionId);
                    await _cartRepository.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                return Result<CartItemDto>.Failure("No valid user or session to attach cart.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var cartItem = cart.AddItem(itemRequest.ProductId, itemRequest.Quantity);

                await _cartRepository.UpdateAsync(cart);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Item added successfully. CartId: {CartId}, ItemId: {ItemId}", cart.Id, cartItem.Id);

                return Result<CartItemDto>.Success(new CartItemDto
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    CartId = cart.Id,
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while adding item to CartId: {CartId}", cart.Id);
                return Result<CartItemDto>.Failure("An error occurred while adding item to the cart.");
            }
        }

    }
}
