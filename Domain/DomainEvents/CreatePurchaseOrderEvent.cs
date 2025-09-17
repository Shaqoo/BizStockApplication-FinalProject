using MediatR;

namespace Domain.DomainEvents
{
    public record CreatePurchaseOrderEvent : INotification
    {
        public Guid PurchaseOrderId { get; set; } 
        public string OrderNumber { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal Tax { get; set; }    
        public decimal Discount { get; set; }
        public List<CreatePurchaseOrderItemEvent> Items { get; set; } = [];
        public CreatePurchaseOrderEvent(Guid purchaseOrderId, string orderNumber, Guid supplierId, DateTime createdAt, DateTime? expectedDeliveryDate, decimal discount, decimal tax)
        {
            PurchaseOrderId = purchaseOrderId;
            OrderNumber = orderNumber;
            SupplierId = supplierId;
            CreatedDate = createdAt;
            ExpectedDeliveryDate = expectedDeliveryDate;
            Tax = tax;
            Discount = discount;
        }
        public void AddItem(CreatePurchaseOrderItemEvent item)
        {
            Items.Add(item);
        }
    }

    public record CreatePurchaseOrderItemEvent(
        Guid ProductId,
        string ProductName,
        int QuantityOrdered,
        decimal UnitPrice
    );

}
