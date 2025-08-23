using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetBySessionId
{
    public class GetCartItemsBySessionIdQueryHandler
    : IRequestHandler<GetCartItemsBySessionIdQuery, Result<PaginatedList<CartItemDto>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetCartItemsBySessionIdQueryHandler> _logger;

        public GetCartItemsBySessionIdQueryHandler(
            ICartRepository cartRepository,
            IMemoryCacheService cache,
            ILogger<GetCartItemsBySessionIdQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<CartItemDto>>> Handle(GetCartItemsBySessionIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _cartRepository.GetBySessionIdAsync(request.SessionId);
            if (item is null)
            {
                _logger.LogWarning("Cart not found for SessionId {SessionId}", request.SessionId);
                return Result<PaginatedList<CartItemDto>>.Failure("Cart not found for session");
            }
            var cacheKey = $"CartItems_SessionId_{request.SessionId}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var items = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching cart items for SessionId {SessionId} from DB", request.SessionId);

                var cart = await _cartRepository.GetCartItemsBySessionIdAsync(request.SessionId,request.PageRequest);
                if (cart is null)
                    return null;
                return cart;
            },TimeSpan.FromMinutes(5));

            return items is null
                ? Result<PaginatedList<CartItemDto>>.Failure("Cart not found for session")
                : Result<PaginatedList<CartItemDto>>.Success(items);
        }
    }

}
