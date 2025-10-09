using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Invoices.GetInvoicesByOrder
{
    public class GetInvoicesByOrderIdQueryHandler(IInvoiceRepository invoiceRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetInvoicesByOrderIdQuery, Result<IEnumerable<InvoiceDto>>>
    {
        public async Task<Result<IEnumerable<InvoiceDto>>> Handle(GetInvoicesByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Invoices_Customer_{request.orderId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var invoices = await invoiceRepository.GetInvoicesByOrderIdAsync(request.orderId);

                var invoiceDtos = invoices.Select(a => new InvoiceDto
                {
                    Id = a.Id,
                    OrderNumber = a.SalesOrder?.OrderNumber ?? "",
                    Discount = a.Discount,
                    SalesOrderId = a.SalesOrderId ?? Guid.Empty,
                    Status = a.Status,
                    SubTotal = a.SubTotal,
                    Tax = a.Tax,
                    CustomerId = a.CustomerId,
                    CustomerName = a.Customer.FullName,
                    DueDate = a.DueDate,
                    InvoiceNumber = a.InvoiceNumber,
                    Items = a.Items.Select(a => new InvoiceItemDto
                    {
                        Description = a.Description,
                        ProductId = a.ProductId,
                        Quantity = a.Quantity,
                        UnitPrice = a.UnitPrice
                    }).ToList()
                }).ToList();

                return Result<IEnumerable<InvoiceDto>>.Success(invoiceDtos);
            }, TimeSpan.FromMinutes(10));

            return cachedResult ?? new Result<IEnumerable<InvoiceDto>>();
        }
    }
}
