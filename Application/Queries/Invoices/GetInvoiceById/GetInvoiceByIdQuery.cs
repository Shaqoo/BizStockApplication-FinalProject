using Application.Dto;
using MediatR;

namespace Application.Queries.Invoices.GetInvoiceById
{
    public record GetInvoiceByIdQuery(Guid invoiceId) : IRequest<Result<InvoiceDto>>;

}
