using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.SalesOrders.GetSalesOrdersByCustomerId
{
    public class GetSalesOrderByCustomerIdQueryHandler(IMemoryCacheService memoryCacheService,
        ISalesOrderRepository salesOrderRepository) : IRequestHandler<GetSalesOrderByCustomerIdQuery, Result<PaginatedList<SalesOrderDto>>>
    {
        public async Task<Result<PaginatedList<SalesOrderDto>>> Handle(GetSalesOrderByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"SalesOrders_Customer_{request.customerId}_Page_{request.PageRequest.Page}_Size_{request.PageRequest.PageSize}Id";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var salesOrders = await salesOrderRepository.GetByCustomerIdAsync(request.customerId, request.PageRequest);
                var salesOrderDtos = salesOrders.Items.Select(so => new SalesOrderDto
                {
                    Id = so.Id,
                    OrderNumber = so.OrderNumber,
                    CustomerId = so.CustomerId,
                    Status = so.Status,
                    SubTotal = so.SubTotal,
                    Tax = so.Tax,
                    Discount = so.Discount,
                    CustomerName = so.Customer.FullName,
                    InvoiceId = so.InvoiceId,
                    Note = so.Note,
                    DeliveryAssignmentId = so.DeliveryAssignmentId,
                    OverallDeliveryStatus = so.OverallDeliveryStatus,
                    Total = so.Total,
                    ExpectedDeliveryDate = so.ExpectedDeliveryDate,
                    DateCreated = so.DateCreated,
                    InvoiceNumber = so.Invoice.InvoiceNumber,
                    Items = so.Items.Select(item => new SalesOrderItemDto
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
                }).ToList();
                var paginatedListDto = new PaginatedList<SalesOrderDto>(salesOrderDtos, salesOrders.TotalCount, salesOrders.PageNumber, salesOrders.PageSize);
                return Result<PaginatedList<SalesOrderDto>>.Success(paginatedListDto);
            }, TimeSpan.FromMinutes(15));

            return cachedResult ?? Result<PaginatedList<SalesOrderDto>>.Success(new PaginatedList<SalesOrderDto>(new List<SalesOrderDto>(), 0, request.PageRequest.Page, request.PageRequest.PageSize));
        }
    }
}
