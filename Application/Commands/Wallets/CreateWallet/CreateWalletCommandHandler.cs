using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Wallets.CreateWallet
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Result<Guid>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateWalletCommandHandler> _logger;

        public CreateWalletCommandHandler(
            ICustomerRepository customerRepository,
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateWalletCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.Request.CustomerId);
                if (customer is null)
                {
                    _logger.LogWarning("Customer {CustomerId} not found when creating wallet", request.Request.CustomerId);
                    return Result<Guid>.Failure("Customer not found");
                }

                var existingWallet = await _walletRepository.GetByUserIdAsync(request.Request.CustomerId);
                if (existingWallet is not null)
                {
                    _logger.LogWarning("Wallet already exists for customer {CustomerId}", request.Request.CustomerId);
                    return Result<Guid>.Failure("Customer already has a wallet");
                }

                var wallet = new Wallet(request.Request.CustomerId);
                wallet.SetPin(BCrypt.Net.BCrypt.HashPassword(request.Request.Pin.ToString()));

                await _walletRepository.AddAsync(wallet);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Wallet {WalletId} created for customer {CustomerId}", wallet.Id, request.Request.CustomerId);

                return Result<Guid>.Success(wallet.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet for customer {CustomerId}", request.Request.CustomerId);
                return Result<Guid>.Failure("An error occurred while creating the wallet");
            }
        }
    }

}
