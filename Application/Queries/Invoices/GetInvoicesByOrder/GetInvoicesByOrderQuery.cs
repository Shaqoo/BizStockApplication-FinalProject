using Application.Dto;
using MediatR;

namespace Application.Queries.Invoices.GetInvoicesByOrder
{
    public record GetInvoicesByOrderIdQuery(Guid orderId) : IRequest<Result<IEnumerable<InvoiceDto>>>;

}
