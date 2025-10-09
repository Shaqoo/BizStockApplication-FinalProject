using Application.Dto;
using MediatR;

namespace Application.Queries.Payments.GetPaymentsByInvoice
{
    public record GetPaymentsByInvoiceQuery(Guid invoiceId)
        :IRequest<Result<List<PaymentDto>>>;
    
}
