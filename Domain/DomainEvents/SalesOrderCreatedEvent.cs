using MediatR;

namespace Domain.DomainEvents
{
    public class OrderCreatedEvent : INotification
    {
        public Guid OrderId { get; }
        public Guid UserId { get; }
        public string OrderNumber { get; }
        public string CustomerEmail { get; }
        public string CustomerName { get; }
        public string DeliveryReference { get; }

        public decimal SubTotal { get; }
        public decimal DeliveryCost { get; }
        public decimal Total { get; }

        public IReadOnlyList<OrderCreatedProductDto> Products { get; }

        public OrderCreatedEvent(
            Guid orderId,
            Guid userId,
            string orderNumber,
            string customerEmail,
            string customerName,
            string deliveryReference,
            decimal subTotal,
            decimal deliveryCost,
            decimal total,
            IReadOnlyList<OrderCreatedProductDto> products)
        {
            OrderId = orderId;
            UserId = userId;
            OrderNumber = orderNumber;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            DeliveryReference = deliveryReference;
            SubTotal = subTotal;
            DeliveryCost = deliveryCost;
            Total = total;
            Products = products;
        }
    }

    public class OrderCreatedProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

}
