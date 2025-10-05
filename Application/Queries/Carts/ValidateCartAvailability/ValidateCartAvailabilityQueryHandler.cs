using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Carts.ValidateCartAvailability
{
    public class ValidateCartAvailabilityQueryHandler : IRequestHandler<ValidateCartAvailabilityQuery, Result<List<CartItemAvailabilityDto>>>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAuthService _authService;

        public ValidateCartAvailabilityQueryHandler(
            ICartRepository cartRepository,
            IAuthService authService,
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _authService = authService;
            _productRepository = productRepository;
        }

        public async Task<Result<List<CartItemAvailabilityDto>>> Handle(ValidateCartAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var user = _authService.CurrentUser();
            if (user == null)
                return Result<List<CartItemAvailabilityDto>>.Failure("User not authenticated");

            var cart = await _cartRepository.GetByUserIdAsync(user.Id);
            if (cart == null || !cart.Items.Any())
                return Result<List<CartItemAvailabilityDto>>.Failure("Cart is empty");

            var availabilityList = new List<CartItemAvailabilityDto>();

            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    return Result<List<CartItemAvailabilityDto>>.Failure($"Product {item.ProductId} not found");

                availabilityList.Add(new CartItemAvailabilityDto
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    RequestedQuantity = item.Quantity,
                    AvailableQuantity = product.StockByWarehouse.Count > 0
                        ? product.StockByWarehouse.Sum(x => x.Quantity)
                        : 0,
                });
            }

             
            if (availabilityList.Any(x => !x.IsAvailable))
                return new Result<List<CartItemAvailabilityDto>>
                {
                  Message = "Some items are not available in requested quantity", 
                  Data = availabilityList ,
                  IsSuccess = false
                };

            return Result<List<CartItemAvailabilityDto>>.Success(availabilityList);
        }
    }

}
