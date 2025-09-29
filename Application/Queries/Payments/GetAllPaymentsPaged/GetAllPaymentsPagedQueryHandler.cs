using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Payments.GetAllPaymentsPaged
{
    public class GetAllPaymentsPagedQueryHandler
    : IRequestHandler<GetAllPaymentsPagedQuery, Result<PaginatedList<PaymentDto>>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ILogger<GetAllPaymentsPagedQueryHandler> _logger;

        public GetAllPaymentsPagedQueryHandler(IPaymentRepository paymentRepository,
                                               IMemoryCacheService memoryCacheService,
                                               ILogger<GetAllPaymentsPagedQueryHandler> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<PaginatedList<PaymentDto>>> Handle(GetAllPaymentsPagedQuery request,
                                                                    CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetAllPaymentsPagedQuery:{request.PageRequest.PageSize}:{request.PageRequest.Page}";

                var cachedResult = await _memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var pagedPayments = await _paymentRepository
                    .GetAllAsync(request.PageRequest);


                    var pagedDto = new PaginatedList<PaymentDto>(
                        pagedPayments.Items.Select(p => p.AsDto()).ToList(),
                        pagedPayments.TotalCount,
                        pagedPayments.PageNumber,
                        request.PageRequest.PageSize
                    );
                    return pagedDto;
                },TimeSpan.FromMinutes(10));

                return Result<PaginatedList<PaymentDto>>.Success(cachedResult ?? new PaginatedList<PaymentDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all payments");
                return Result<PaginatedList<PaymentDto>>.Failure("Failed to retrieve payments.");
            }
        }
    }

}
