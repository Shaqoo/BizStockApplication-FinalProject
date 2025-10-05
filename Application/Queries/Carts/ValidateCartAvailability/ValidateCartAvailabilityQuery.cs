using Application.Dto;
using MediatR;

namespace Application.Queries.Carts.ValidateCartAvailability
{
    public record ValidateCartAvailabilityQuery : IRequest<Result<List<CartItemAvailabilityDto>>>;

    public class CartItemAvailabilityDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int RequestedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool IsAvailable => RequestedQuantity <= AvailableQuantity - 2;
    }

}
