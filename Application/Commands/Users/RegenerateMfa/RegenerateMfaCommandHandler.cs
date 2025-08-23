using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.Service.Application.Common.Interfaces;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.RegenerateMfa
{
    public class RegenerateMfaCommandHandler : IRequestHandler<RegenerateMfaCommand, Result<TwoFactorSetupDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMfaService _mfaService;
        private readonly IRecoveryCodeGenerator _recoveryCodeGenerator;
        private readonly IAuthService _authService;
        private readonly ILogger<RegenerateMfaCommandHandler> _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegenerateMfaCommandHandler(
            IUserRepository userRepository,
            IMfaService mfaService,
            IRecoveryCodeGenerator recoveryCodeGenerator,
            IAuthService authService,
            ILogger<RegenerateMfaCommandHandler> logger,
            IAuditLogRepository auditLogRepository,
            IPublishEndpoint publishEndpoint,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mfaService = mfaService;
            _recoveryCodeGenerator = recoveryCodeGenerator;
            _authService = authService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _auditLogRepository = auditLogRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<TwoFactorSetupDto>> Handle(RegenerateMfaCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _authService.CurrentUser();
            if (currentUser is null)
            {
                _logger.LogWarning("Unauthorized access attempt to regenerate MFA by an unauthenticated user.");
                return Result<TwoFactorSetupDto>.Failure("Unauthorized");
            }

            var user = await _userRepository.GetByIdAsync(currentUser.Id);
            if (user is null)
            {
                _logger.LogWarning($"User with ID {currentUser.Id} not found while attempting to regenerate MFA.");
                return Result<TwoFactorSetupDto>.Failure("User not found");
            }

            var mfa = await _mfaService.ResetMfaAsync(user);

            var recoveryCodes = _recoveryCodeGenerator.Generate(10);
            _logger.LogInformation($"Regenerating MFA for user {user.Email}. New recovery codes generated.");
            user.ClearRecoveryCodes();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //await _userRepository.UpdateUserAsync(user);
                foreach (var code in recoveryCodes)
                {
                    await _userRepository.AddCode(new UserRecoveryCode(user.Id, RecoveryCodeHasher.Hash(code)));
                }
                var auditLog = new AuditLog
                (
                    userId:user.Id,
                    action:"RegenerateMFA",
                    entityName:nameof(User),
                    entityId:user.Id,
                    details:$"User {user.Email} regenerated MFA and recovery codes.",
                    ip:request.RequestMetadata?.IpAddress,
                    userAgent:request.RequestMetadata?.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();
                await _publishEndpoint.Publish(new MfaResetEvent(
                    user.Id,
                    (string)user.Email,
                    user.FullName,
                    DateTime.UtcNow
                ));


                return Result<TwoFactorSetupDto>.Success(new TwoFactorSetupDto
                {
                    ManualEntryKey = mfa.ManualEntryKey,
                    QrCodeImageUrl = mfa.QrCodeImageUrl,
                    RecoveryCodes = recoveryCodes
                }, "MFA reconfigured successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while regenerating MFA for user {UserId}", user.Id);
                return Result<TwoFactorSetupDto>.Failure("Failed to regenerate MFA");
            }
        }
    }
}
