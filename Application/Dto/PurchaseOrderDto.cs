using Domain.Enums;

namespace Application.Dto
{
    public class PurchaseOrderStatsDto
    {
        public int TotalPurchaseOrders { get; set; }
        public int DraftCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int ReceivedCount { get; set; }
        public int CancelledCount { get; set; }
        public int RejectedCount { get; set; }
        public int PartiallyReceivedCount { get; set; }
        public decimal TotalSpend { get; set; }
        public decimal OutstandingAmount { get; set; } 
    }

    public class PurchaseOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImgUrl { get; set; } = string.Empty;
        public int OrderedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => OrderedQuantity * UnitPrice;
    }

    public class PurchaseOrderDetailDto
    {
        public Guid Id { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public PurchaseOrderStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public decimal TotalAmount { get; set; }

        public List<PurchaseOrderItemDto> Items { get; set; } = new();
    }

    public class PurchaseOrderListDto
    {
        public Guid Id { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public PurchaseOrderStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }



}
