using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Application.Common.Interfaces;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Application.Commands.Customers.Create
{
    public class CreateCustomerHandler(IUserRepository userRepository,
        IAuditLogRepository logRepository,
        IMfaService mfaService,
        ICustomerRepository customerRepository,
        ICustomerTypeRepository customerTypeRepository,
        IUnitOfWork unitOfWork,
        IRecoveryCodeGenerator recoveryCodeGenerator,
        IMediator mediator,
        IMemoryCacheService memoryCacheService,
        ILogger<CreateCustomerHandler> logger,
        IWalletRepository walletRepository,
        IPublishEndpoint pubishEndpoint
        ) : IRequestHandler<CreateCustomerCommand, Result<TwoFactorSetupDto>>
    {
        public async Task<Result<TwoFactorSetupDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Model == null)
            {
                return Result<TwoFactorSetupDto>.Failure("Invalid request payload.");
            }

            var isUnique = await userRepository.IsEmailUniqueAsync(request.Model.Email);
            if (!isUnique)
            {
                return Result<TwoFactorSetupDto>.Failure("Email is already in use.");
            }

            var salt = RandomNumberGenerator.GetBytes(64);
            var base64Salt = Convert.ToBase64String(salt);

            var password = PasswordHasher.HashPassword(request.Model.Password,base64Salt);
            var dob = DateTime.SpecifyKind(request.Model.BirthDate, DateTimeKind.Utc);

            string fullName = $"{request.Model.FirstName?.Trim()} {request.Model.LastName?.Trim()}".Trim();


            var user = new User(Email.Create(request.Model.Email), password,base64Salt,PhoneNumber.Create(request.Model.PhoneNumber) 
                ,request.Model.Gender, DateOfBirth.Create(dob),fullName);
            

            var customerType = await customerTypeRepository.GetByNameAsync(request.Model.CustomerType);
            if (customerType == null)
            {
                return Result<TwoFactorSetupDto>.Failure("Invalid customer type selected.");
            }

            var customer = new Customer(Email.Create(request.Model.Email),customerType.Id,fullName,
                 request.Model.BusinessName,request.Model.Address,request.Model.TaxId);
            Console.WriteLine($"Customer FullName: '{customer.FullName}' ({customer.FullName?.Length})");
            Console.WriteLine($"Customer BusinessName: '{customer.BusinessName}' ({customer.BusinessName?.Length})");
            Console.WriteLine($"User FullName: '{user.FullName}' ({user.FullName?.Length})");
            Console.WriteLine($"User ProfilePictureUrl: '{user.ProfilePictureUrl}' ({user.ProfilePictureUrl?.Length})");

            user.AddRole(Role.Customer);

            var recoveryCodes = recoveryCodeGenerator.Generate(10);
            foreach (var code in recoveryCodes)
            {
                string hashedCode = RecoveryCodeHasher.Hash(code);
                user.AddRecoveryCode(new UserRecoveryCode(user.Id, hashedCode));
            }

            logger.LogInformation("Generated new recovery codes for user {UserId}", user.Id);
            await logRepository.AddAsync(new AuditLog(
                user.Id,
                "RecoveryCodesGenerated",
                "UserRecoveryCode",
                null,
                "Generated new set of recovery codes",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

            var wallet = new Wallet(customer.Id);
            wallet.SetPin(BCrypt.Net.BCrypt.HashPassword(request.Model.Pin.ToString()));
            customer.AddWallet(wallet.Id);

            var mfa = await mfaService.GenerateSecretAndQrAsync(user);
            try
            {
                await unitOfWork.BeginTransactionAsync();
                await userRepository.AddAsync(user);
                await customerRepository.AddAsync(customer);
                await walletRepository.AddAsync(wallet);

                var auditLog = new AuditLog(
                user.Id,
                "CreateCustomer",
                "Customer",
                customer.Id,
                $"Customer '{user.FullName}' was created.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            );

                await logRepository.AddAsync(auditLog);

                await memoryCacheService.RemoveCacheByPrefix("SearchUser", "GetUserById", "GetUserByEmail", "AllUser");

                await unitOfWork.CommitTransactionAsync();
                await mediator.Publish(new UserRegisteredEvent(user.Id, (string)user.Email, fullName,mfa.ManualEntryKey ,mfa.QrCodeImageUrl), cancellationToken);
                await pubishEndpoint.Publish(new UserRegisteredEvent(user.Id, (string)user.Email, fullName,mfa.ManualEntryKey ,mfa.QrCodeImageUrl), cancellationToken);
                return Result<TwoFactorSetupDto>.Success(new TwoFactorSetupDto
                {
                    ManualEntryKey = mfa.ManualEntryKey,
                    QrCodeImageUrl = mfa.QrCodeImageUrl,
                    RecoveryCodes = recoveryCodes
                },"Registration Successful");
            }
            catch (Exception ex)
            {
                var failureLog = new AuditLog(
                   userId: Guid.Empty, 
                   action: "CreateCustomerFailed",
                   entityName: "Customer",
                   entityId: null,
                   details: $"Customer creation failed for email '{request.Model.Email}': {ex.Message}",
                   ip: request.RequestMetadata.IpAddress,
                   userAgent: request.RequestMetadata.UserAgent
                );
                await logRepository.AddAsync(failureLog);

                Console.WriteLine(ex.Message);
                await unitOfWork.RollbackTransactionAsync();
                return Result<TwoFactorSetupDto>.Failure("An unexpected error occurred while creating the customer."); 
            }
        }
         
    }
}
