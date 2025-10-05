using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Invoices.GetInvoicesByCustomer
{
    public class GetInvoicesByCustomerQueryHandler(IInvoiceRepository invoiceRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetInvoicesByCustomerQuery, Result<PaginatedList<InvoiceDto>>>
    {
        public async Task<Result<PaginatedList<InvoiceDto>>> Handle(GetInvoicesByCustomerQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Invoices_Customer_{request.customerId}_Page_{request.PageRequest.Page}_Size_{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var invoices = await invoiceRepository.GetByCustomerIdAsync(request.customerId, request.PageRequest);

                var invoiceDtos = invoices.Items.Select(a => new InvoiceDto
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

                return Result<PaginatedList<InvoiceDto>>.Success(new PaginatedList<InvoiceDto>(invoiceDtos,invoices.TotalCount,invoices.PageNumber,invoices.PageSize));
            },TimeSpan.FromMinutes(10));

            return cachedResult ?? new Result<PaginatedList<InvoiceDto>>();
        }
    }
}
