using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.SalesOrders.GetSalesOrderById
{
    public class GetSalesOrderByIdQueryHandler(ISalesOrderRepository salesOrderRepository,
        IMemoryCacheService memoryCacheService)
        : IRequestHandler<GetSalesOrderByIdQuery, Result<SalesOrderDto>>
    {
        public async Task<Result<SalesOrderDto>> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
        {
             var cacheKey = $"SalesOrder_{request.salesOrderId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var salesOrder = await salesOrderRepository.GetWithItemsAsync(request.salesOrderId);
                if (salesOrder == null)
                {
                    return Result<SalesOrderDto>.Failure("Sales order not found.");
                }
                var salesOrderDto = new SalesOrderDto
                {
                    Id = salesOrder.Id,
                    OrderNumber = salesOrder.OrderNumber,
                    CustomerId = salesOrder.CustomerId,
                    Status = salesOrder.Status,
                    SubTotal = salesOrder.SubTotal,
                    Tax = salesOrder.Tax,
                    Discount = salesOrder.Discount,
                    CustomerName = salesOrder.Customer.FullName,
                    InvoiceId = salesOrder.InvoiceId,
                    Note = salesOrder.Note,
                    DeliveryAssignmentId = salesOrder.DeliveryAssignmentId,
                    OverallDeliveryStatus = salesOrder.OverallDeliveryStatus,
                    Total = salesOrder.Total,
                    ExpectedDeliveryDate = salesOrder.ExpectedDeliveryDate,
                    DateCreated = salesOrder.DateCreated,
                    InvoiceNumber = salesOrder.Invoice.InvoiceNumber,
                    Items = salesOrder.Items.Select(item => new SalesOrderItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        DeliveryStatus = item.DeliveryStatus,
                        FezOrderNo = item.FezOrderNo,
                        ProductName = item.ProductName,
                        SalesOrderId = item.SalesOrderId,
                        TotalPrice = item.TotalPrice,
                        UniqueId = item.UniqueId,
                    }).ToList()
                };
                return Result<SalesOrderDto>.Success(salesOrderDto);
            }, TimeSpan.FromMinutes(5)); 

            return cachedResult ?? Result<SalesOrderDto>.Success(new SalesOrderDto());
        }
    }
}
