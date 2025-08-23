using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetByUserId
{
    public class GetCartItemsByUserIdQueryHandler
    : IRequestHandler<GetCartItemsByUserIdQuery, Result<PaginatedList<CartItemDto>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetCartItemsByUserIdQueryHandler> _logger;

        public GetCartItemsByUserIdQueryHandler(
            ICartRepository cartRepository,
            IMemoryCacheService cache,
            ILogger<GetCartItemsByUserIdQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<CartItemDto>>> Handle(GetCartItemsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _cartRepository.GetByUserIdAsync(request.UserId);
            if (item is null)
            {
                _logger.LogWarning("Cart not found for UserId {UserId}", request.UserId);
                return Result<PaginatedList<CartItemDto>>.Failure("Cart not found for user");
            }

            var cacheKey = $"CartItems_UserId_{request.UserId}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var items = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching cart items for UserId {UserId} from DB", request.UserId);

                var cart = await _cartRepository.GetCartItemsByUserIdAsync(request.UserId,request.PageRequest);
                if (cart is null)
                    return null;
                return cart;
            });

            return items is null
                ? Result<PaginatedList<CartItemDto>>.Failure("Cart not found for user")
                : Result<PaginatedList<CartItemDto>>.Success(items);
        }
    }

}
