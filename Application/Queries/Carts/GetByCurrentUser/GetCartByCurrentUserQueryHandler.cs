using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetByCurrentUser
{
    public class GetCartByCurrentUserQueryHandler
        : IRequestHandler<GetCartByCurrentUserQuery, Result<PaginatedCartDto>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<GetCartByCurrentUserQueryHandler> _logger;

        public GetCartByCurrentUserQueryHandler(
            ICartRepository cartRepository,
            IAuthService authService,
            ILogger<GetCartByCurrentUserQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<PaginatedCartDto>> Handle(GetCartByCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _authService.CurrentUser()!.Id;

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.LogWarning("Cart for User {UserId} not found", userId);
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
