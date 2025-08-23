using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Carts.GetById
{
    public class GetCartItemsQueryHandler
    : IRequestHandler<GetCartItemsQuery, Result<PaginatedList<CartItemDto>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetCartItemsQueryHandler> _logger;

        public GetCartItemsQueryHandler(
            ICartRepository cartRepository,
            IMemoryCacheService cache,
            ILogger<GetCartItemsQueryHandler> logger)
        {
            _cartRepository = cartRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<CartItemDto>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"CartItems_CartId_{request.CartId}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var items = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Fetching cart items for CartId {CartId} from DB", request.CartId);

                var cart = await _cartRepository.GetCartItemsAsync(request.CartId,request.PageRequest);
                if (cart is null)
                    return null;
                return cart;
            },TimeSpan.FromMinutes(5));

            return items is null
                ? Result<PaginatedList<CartItemDto>>.Failure("Cart not found")
                : Result<PaginatedList<CartItemDto>>.Success(items);
        }
    }

}
