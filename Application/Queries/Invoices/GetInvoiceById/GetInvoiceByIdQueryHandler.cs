using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Invoices.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler(IMemoryCacheService memoryCacheService,
        IInvoiceRepository invoiceRepository) : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDto>>
    {
        public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetInvoiceByIdQuery:{request.invoiceId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var invoice = await invoiceRepository.GetByIdAsync(request.invoiceId);
                if (invoice == null)
                {
                    return Result<InvoiceDto>.Failure("Invoice Not Found");
                }
                var invoiceDto = new InvoiceDto
                {
                    CustomerId = invoice.CustomerId,
                    CustomerName = invoice.Customer.FullName,
                    Discount = invoice.Discount,
                    DueDate = invoice.DueDate,
                    Id = invoice.Id,
                    InvoiceNumber = invoice.InvoiceNumber,
                    OrderNumber = invoice.SalesOrder?.OrderNumber ?? "",
                    SalesOrderId = invoice.SalesOrderId ?? Guid.Empty,
                    Status = invoice.Status,
                    SubTotal = invoice.SubTotal,
                    Tax = invoice.Tax,
                    Items = invoice.Items.Select(a => new InvoiceItemDto
                    {
                        Description = a.Description,
                        ProductId = a.ProductId,
                        Quantity = a.Quantity,
                        UnitPrice = a.UnitPrice
                    }).ToList()
                };
                return Result<InvoiceDto>.Success(invoiceDto);
            },TimeSpan.FromMinutes(20));

            return cachedResult ?? Result<InvoiceDto>.Success(new InvoiceDto());
        }
    }
}
