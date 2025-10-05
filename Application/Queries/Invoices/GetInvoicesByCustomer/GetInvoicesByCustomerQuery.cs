using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Invoices.GetInvoicesByCustomer
{
    public record GetInvoicesByCustomerQuery(PageRequest PageRequest, Guid customerId) : IRequest<Result<PaginatedList<InvoiceDto>>>;

}
