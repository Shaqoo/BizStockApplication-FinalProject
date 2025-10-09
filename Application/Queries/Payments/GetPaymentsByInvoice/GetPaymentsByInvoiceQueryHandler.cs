using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.Payments.GetPaymentsByInvoice
{
    public class GetPaymentsByInvoiceQueryHandler(IMemoryCacheService memoryCacheService,
        IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentsByInvoiceQuery, Result<List<PaymentDto>>>
    {
        public async Task<Result<List<PaymentDto>>> Handle(GetPaymentsByInvoiceQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetPaymentsByInvoiceQuery:{request.invoiceId}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey ,async () =>
            {
                var payments = await paymentRepository.GetByInvoiceIdAsync(request.invoiceId);

                var paymentsDto = payments.Select(payment => new PaymentDto
                {
                    InvoiceId = payment.InvoiceId,
                    Amount = payment.Amount,
                    CreatedAt = payment.DateCreated,
                    Id = payment.Id,
                    Method = payment.Method,
                    Note = payment.Note,
                    PayerId = payment.PayerId,
                    PayerName = payment.Payer.FullName,
                    PaymentReference = payment.PaymentReference,
                    Purpose = payment.Purpose,
                    Status = payment.Status
                }).ToList();

                return paymentsDto;
            });

            return Result<List<PaymentDto>>.Success(cachedResult ?? []);
        }
    }
}
