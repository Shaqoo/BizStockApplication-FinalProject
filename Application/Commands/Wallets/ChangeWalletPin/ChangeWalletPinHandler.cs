using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Wallets.ChangeWalletPin
{
    public class ChangeWalletPinCommandHandler : IRequestHandler<ChangeWalletPinCommand, Result<bool>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChangeWalletPinCommandHandler> _logger;

        public ChangeWalletPinCommandHandler(
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            ILogger<ChangeWalletPinCommandHandler> logger)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(ChangeWalletPinCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _walletRepository.GetByIdAsync(request.Request.WalletId);
                if (wallet is null)
                {
                    _logger.LogWarning("Wallet {WalletId} not found when changing PIN", request.Request.WalletId);
                    return Result<bool>.Failure("Wallet not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Request.OldPin.ToString(), wallet.PinHash))
                {
                    _logger.LogWarning("Incorrect old PIN for wallet {WalletId}", request.Request.WalletId);
                    return Result<bool>.Failure("Old PIN is incorrect");
                }

                wallet.SetPin(BCrypt.Net.BCrypt.HashPassword(request.Request.NewPin.ToString()));
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Wallet {WalletId} PIN updated", wallet.Id);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing PIN for wallet {WalletId}", request.Request.WalletId);
                return Result<bool>.Failure("An error occurred while changing the wallet PIN");
            }
        }
    }
}
