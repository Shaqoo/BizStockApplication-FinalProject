using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Wallets.GetWalletByCustomerId
{
    public class GetWalletByCustomerIdQueryHandler
        : IRequestHandler<GetWalletByCustomerIdQuery, Result<WalletDto>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetWalletByCustomerIdQueryHandler> _logger;

        public GetWalletByCustomerIdQueryHandler(
            IWalletRepository walletRepository,
            IMemoryCacheService cache,
            ILogger<GetWalletByCustomerIdQueryHandler> logger)
        {
            _walletRepository = walletRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<WalletDto>> Handle(GetWalletByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"wallet_customer_{request.CustomerId}";

                var wallet = await _cache.GetOrAddAsync(
                    cacheKey,
                    async () => await _walletRepository.GetByUserIdAsync(request.CustomerId),
                    TimeSpan.FromMinutes(1)
                );

                if (wallet is null)
                {
                    _logger.LogWarning("Wallet not found for customer {CustomerId}", request.CustomerId);
                    return Result<WalletDto>.Failure("Wallet not found");
                }

                var dto = new WalletDto
                {
                    Id = wallet.Id,
                    CustomerId = wallet.CustomerId,
                    Balance = wallet.Balance,
                    IsActive = wallet.IsActive
                };

                return Result<WalletDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching wallet for customer {CustomerId}", request.CustomerId);
                return Result<WalletDto>.Failure("An error occurred while fetching the wallet");
            }
        }
    }

}
