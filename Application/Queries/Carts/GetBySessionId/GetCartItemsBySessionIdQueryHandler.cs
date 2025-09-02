using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetBySessionId
{
    public class GetCartBySessionIdQueryHandler
        : IRequestHandler<GetCartBySessionIdQuery, Result<PaginatedCartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<GetCartBySessionIdQueryHandler> _logger;

        public GetCartBySessionIdQueryHandler(
            ICartRepository cartRepository,
            ILogger<GetCartBySessionIdQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<Result<PaginatedCartDto>> Handle(
            GetCartBySessionIdQuery request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetBySessionIdAsync(request.SessionId);

            if (cart == null)
            {
                _logger.LogWarning("Cart with SessionId {SessionId} not found", request.SessionId);
                return Result<PaginatedCartDto>.Failure("Cart not found.");
            }

            var items = await _cartRepository.GetCartItemsAsync(cart.Id, request.PageRequest);
            var totalQuantities = await _cartRepository.GetTotalCountAsync(cart.Id);
            var totalPrice = await _cartRepository.GetTotalPriceAsync(cart.Id);

            var cartDto = new PaginatedCartDto
            {
                Id = cart.Id,
                IsLinked = cart.IsLinked,
                Items = new PaginatedList<CartItemDto>
                {
                    Items = items.Items.ToDtoList().ToList(),
                    PageNumber = items.PageNumber,
                    PageSize = items.PageSize,
                    TotalCount = items.TotalCount
                },
                SessionId = cart.SessionId,
                UserId = cart.UserId,
                TotalQuantity = totalQuantities,
                TotalPrice = totalPrice
            };

            return Result<PaginatedCartDto>.Success(cartDto);
        }
    }
}
