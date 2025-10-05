namespace Domain.DomainEvents
{
    using MediatR;

    public class StockReservedEvent : INotification
    {
        public Guid SalesOrderId { get; }
        public IReadOnlyList<StockItemDto> Items { get; }
        public DateTime ReservedAt { get; }

        public StockReservedEvent(Guid salesOrderId, IReadOnlyList<StockItemDto> items)
        {
            SalesOrderId = salesOrderId;
            Items = items;
            ReservedAt = DateTime.UtcNow;
        }
    }

   

    public class StockRestoredEvent : INotification
    {
        public Guid SalesOrderId { get; }
        public IReadOnlyList<StockItemDto> Items { get; }
        public DateTime RestoredAt { get; }

        public StockRestoredEvent(Guid salesOrderId, IReadOnlyList<StockItemDto> items)
        {
            SalesOrderId = salesOrderId;
            Items = items;
            RestoredAt = DateTime.UtcNow;
        }
    }


    public record StockItemDto(
        Guid ProductId,
        string ProductName,
        int Quantity
    );

}
