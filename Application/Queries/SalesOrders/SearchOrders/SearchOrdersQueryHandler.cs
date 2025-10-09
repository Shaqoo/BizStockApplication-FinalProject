using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Domain.Entities;
using MediatR;

namespace Application.Queries.SalesOrders.SearchOrders
{
    public class SearchOrdersQueryHandler(IMemoryCacheService memoryCacheService,
        ISalesOrderRepository salesOrderRepository) : IRequestHandler<SearchOrdersQuery, Result<PaginatedList<SalesOrderDto>>>
    {
        public async Task<Result<PaginatedList<SalesOrderDto>>> Handle(SearchOrdersQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"SearchOrders_{request.query}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var salesOrders = await salesOrderRepository.Search(request.query, request.PageRequest);
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
                    DeliveredAt = so.DeliveryAssignment.DeliveredAt,
                    DeliveryFee = so.DeliveryAssignment.DeliveryFee,
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
                        ProductImg = item.Product.ImageUrl,
                        FezOrderNo = item.FezOrderNo,
                        ProductName = item.ProductName,
                        SalesOrderId = item.SalesOrderId,
                        TotalPrice = item.TotalPrice,
                        UniqueId = item.UniqueId,
                    }).ToList()
                }).ToList();
                var paginatedListDto = new PaginatedList<SalesOrderDto>(salesOrderDtos, salesOrders.TotalCount, salesOrders.PageNumber, salesOrders.PageSize);
                return Result<PaginatedList<SalesOrderDto>>.Success(paginatedListDto);
            }, TimeSpan.FromMinutes(5));

            return cachedResult ?? Result<PaginatedList<SalesOrderDto>>.Success(new PaginatedList<SalesOrderDto>(new List<SalesOrderDto>(), 0, request.PageRequest.Page, request.PageRequest.PageSize));
        }
    }
}
