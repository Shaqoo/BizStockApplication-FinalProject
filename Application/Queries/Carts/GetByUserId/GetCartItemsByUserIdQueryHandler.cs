using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Pagination;
using Application.Queries.Carts.GetByUserId;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetByUser
{
    public class GetCartByUserIdQueryHandler
        : IRequestHandler<GetCartByUserIdQuery, Result<PaginatedCartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<GetCartByUserIdQueryHandler> _logger;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository, ILogger<GetCartByUserIdQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        public async Task<Result<PaginatedCartDto>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetByUserIdAsync(request.UserId);
            if (cart == null)
            {
                _logger.LogWarning("Cart for User {UserId} not found", request.UserId);
                return Result<PaginatedCartDto>.Failure("Cart not found.");
            }

            var items = await _cartRepository.GetCartItemsAsync(cart.Id, request.PageRequest);

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
                UserId = cart.UserId
            };

            return Result<PaginatedCartDto>.Success(cartDto);
        }
    }
}
