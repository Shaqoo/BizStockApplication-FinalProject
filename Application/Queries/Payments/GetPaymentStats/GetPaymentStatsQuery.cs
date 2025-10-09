using Application.Dto;
using MediatR;

namespace Application.Queries.Payments.GetPaymentStats
{
    public record GetPaymentStatsQuery() : IRequest<Result<PaymentStatsDto>>;

    public record PaymentStatsDto
    {
        public int TotalPaymentCount { get; set; }
        public int SuccessfulPaymentCount { get; set; }
        public int FailedPaymentCount { get; set; }
        public int PendingPaymentCount { get; set; }
    }

}
