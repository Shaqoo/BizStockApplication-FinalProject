using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Invoices.GetInvoices
{
    public record GetInvoicesQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedList<InvoiceDto>>>;

}
