using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetById
{
    public class GetCartByIdQueryHandler
        : IRequestHandler<GetCartItemsQuery, Result<PaginatedCartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<GetCartByIdQueryHandler> _logger;

        public GetCartByIdQueryHandler(
            ICartRepository cartRepository,
            ILogger<GetCartByIdQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<Result<PaginatedCartDto>> Handle(
            GetCartItemsQuery request,
            CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByIdAsync(request.CartId);

            if (cart is null)
            {
                _logger.LogWarning("Cart with ID {CartId} not found", request.CartId);
                return Result<PaginatedCartDto>.Failure("Cart not found.");
            }

            var items = await _cartRepository.GetCartItemsAsync(cart.Id, request.PageRequest);

            var cartDto = new PaginatedCartDto
            {
                Id = cart.Id,
                IsLinked = cart.IsLinked,
                SessionId = cart.SessionId,
                UserId = cart.UserId,
                Items = new PaginatedList<CartItemDto>
                {
                    Items = items.Items.Select(i => i.ToDto()).ToList(),
                    PageNumber = items.PageNumber,
                    PageSize = items.PageSize,
                    TotalCount = items.TotalCount
                }
            };

            return Result<PaginatedCartDto>.Success(cartDto);
        }
    }
}
