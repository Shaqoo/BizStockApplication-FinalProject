using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Payments.GetAllPaymentsPaged
{
    public record GetAllPaymentsPagedQuery(PageRequest PageRequest)
    : IRequest<Result<PaginatedList<PaymentDto>>>;

}

