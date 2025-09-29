using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Payments.GetPaymentsByCustomer
{
    public record GetPaymentsByCustomerQuery(Guid CustomerId, PageRequest PageRequest)
    : IRequest<Result<PaginatedList<PaymentDto>>>;

}
