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

namespace Application.Commands.CustomerService.Create
{
    public class CreateCustomerServiceHandler(IUserRepository userRepository,
        IMediator mediator,
        IAuditLogRepository logRepository,
        IUnitOfWork unitOfWork,
        IMfaService mfaService,
        IPublishEndpoint pubishEndpoint,
        ILogger<CreateCustomerServiceHandler> logger,
        IRecoveryCodeGenerator recoveryCodeGenerator,
        IMemoryCacheService memoryCacheService
        ) : IRequestHandler<CreateCustomerServiceCommand, Result<TwoFactorSetupDto>>
    {
        public async Task<Result<TwoFactorSetupDto>> Handle(CreateCustomerServiceCommand request, CancellationToken cancellationToken)
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

            var password = PasswordHasher.HashPassword(request.Model.Password, base64Salt);
            var dob = DateTime.SpecifyKind(request.Model.Dob, DateTimeKind.Utc);

            string fullName = $"{request.Model.FirstName?.Trim()} {request.Model.LastName?.Trim()}".Trim();


            var user = new User(Email.Create(request.Model.Email), password, base64Salt, PhoneNumber.Create(request.Model.PhoneNumber)
                , request.Model.Gender, DateOfBirth.Create(dob), fullName);

            user.AddRole(Role.CustomerService);

            var mfa = await mfaService.GenerateSecretAndQrAsync(user);
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


            try
            {
                await unitOfWork.BeginTransactionAsync();
                await userRepository.AddAsync(user);
                await unitOfWork.CommitTransactionAsync();

                var auditLog = new AuditLog(
                user.Id,
                "CreateCustomerServiceOfficer",
                "User",
                user.Id,
                $"CustomerServiceOfficer '{user.FullName}' was created.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            );

                await memoryCacheService.RemoveCacheByPrefix("SearchUser", "GetUserById", "GetUserByEmail", "AllUser");
                await logRepository.AddAsync(auditLog);

                await mediator.Publish(new UserRegisteredEvent(user.Id, (string)user.Email, user.FullName, mfa.ManualEntryKey, mfa.QrCodeImageUrl), cancellationToken);
                await pubishEndpoint.Publish(new UserRegisteredEvent(user.Id, (string)user.Email, user.FullName, mfa.ManualEntryKey, mfa.QrCodeImageUrl), cancellationToken);

                return Result<TwoFactorSetupDto>.Success(new TwoFactorSetupDto
                {
                    ManualEntryKey = mfa.ManualEntryKey,
                    QrCodeImageUrl = mfa.QrCodeImageUrl,
                    RecoveryCodes = recoveryCodes
                }, "Registration Successful");
            }
            catch (Exception ex)
            {
                var failureLog = new AuditLog(
                   userId: Guid.Empty,
                   action: "CreateCustomerServiceOfficerFailed",
                   entityName: "User",
                   entityId: null,
                   details: $"Customer Service Officer creation failed for email '{request.Model.Email}': {ex.Message}",
                   ip: request.RequestMetadata.IpAddress,
                   userAgent: request.RequestMetadata.UserAgent
                );
                await logRepository.AddAsync(failureLog);

                Console.WriteLine(ex.Message);
                await unitOfWork.RollbackTransactionAsync();
                return Result<TwoFactorSetupDto>.Failure("An unexpected error occurred while creating the customer service officer."); ;
            }

        }
    }
}
