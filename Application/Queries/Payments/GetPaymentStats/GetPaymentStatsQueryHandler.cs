using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Payments.GetPaymentStats
{
    public class GetPaymentStatsQueryHandler(IPaymentRepository paymentRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetPaymentStatsQuery, Result<PaymentStatsDto>>
    {
        public async Task<Result<PaymentStatsDto>> Handle(GetPaymentStatsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "payment_stats";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () => await paymentRepository.GetPaymentStatsAsync(),
                TimeSpan.FromMinutes(5));
            return Result<PaymentStatsDto>.Success(cachedResult ?? new PaymentStatsDto());
        }
    }
}
