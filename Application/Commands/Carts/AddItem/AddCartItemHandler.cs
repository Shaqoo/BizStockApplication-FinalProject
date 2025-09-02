using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Application.Commands.Carts.AddItem
{
    public class AddCartItemCommandHandler
        : IRequestHandler<AddCartItemCommand, Result<CartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCartItemCommandHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICartItemRepository _cartItemRepository;
        private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _cartSemaphores = new();

        private SemaphoreSlim GetCartSemaphore(Guid cartId)
        {
            return _cartSemaphores.GetOrAdd(cartId, _ => new SemaphoreSlim(1, 1));
        }



        public AddCartItemCommandHandler(
            ICartItemRepository cartItemRepository,
            ICartRepository cartRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddCartItemCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService,
            IProductRepository productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _contextAccessor = httpContextAccessor;
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _productRepository = productRepository;
            _authService = authService;
        }

        public async Task<Result<CartDto>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var itemRequest = request.AddCartItemRequest;

            var product = await _productRepository.GetByIdAsync(itemRequest.ProductId);

            if (product == null)
            {
                _logger.LogWarning("Product not found with Id: {ProductId}", itemRequest.ProductId);
                return Result<CartDto>.Failure("Product not found.");
            }

            var currentUser = _authService.CurrentUser();
            var sessionId = CartSessionExtension.GetOrCreateCartSessionId(_contextAccessor.HttpContext!);
            Cart? cart;

            if (itemRequest.CartId.HasValue && itemRequest.CartId != Guid.Empty)
            {
                cart = await _cartRepository.GetByIdAsync(itemRequest.CartId.Value);
            }
            else if (currentUser is not null)
            {
                cart = await _cartRepository.GetByUserIdAsync(currentUser.Id);

                if (cart == null)
                {
                    cart = new Cart(currentUser.Id);
                    await _cartRepository.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else if (sessionId is not null)
            {
                cart = await _cartRepository.GetBySessionIdAsync(sessionId);

                if (cart == null)
                {
                    cart = new Cart(sessionId);
                    await _cartRepository.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                return Result<CartDto>.Failure("No valid user or session to attach cart.");
            }
            var semaphore = GetCartSemaphore(cart!.Id);

            await semaphore.WaitAsync();
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var cartItem = await _cartItemRepository
                    .GetByExpression(a => a.CartId == cart!.Id && a.ProductId == itemRequest.ProductId);
                if (cartItem == null)
                {
                    cartItem = new CartItem(cart!.Id, itemRequest.ProductId, itemRequest.Quantity > 0 ? itemRequest.Quantity : 1);
                    await _cartItemRepository.AddAsync(cartItem);
                }
                else
                {
                    if (itemRequest.Quantity != 1)
                        cartItem!.SetQuantity(itemRequest.Quantity);
                    else
                        cartItem!.IncreaseQuantity(1);

                }

                await _unitOfWork.CommitTransactionAsync();

                var total = await _cartRepository.GetTotalCountAsync(cart!.Id);
                var dto = cart.ToDto();
                dto.TotalQuantity = total;

                _logger.LogInformation("Item added successfully. CartId: {CartId}, ItemId: {ItemId}", cart.Id, cartItem.Id);

                return Result<CartDto>.Success(dto);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Concurrency error while adding item to CartId: {CartId}", cart!.Id);
                return Result<CartDto>.Failure("Failed to add item due to a concurrency conflict. Please try again.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while adding item to CartId: {CartId}", cart!.Id);
                return Result<CartDto>.Failure("An error occurred while adding item to the cart.");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
