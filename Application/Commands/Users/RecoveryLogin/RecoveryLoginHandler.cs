using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application.Commands.Users.RecoveryLogin
{
    public class RecoveryLoginHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        ILogger<RecoveryLoginHandler> logger,
        IAuditLogRepository auditLogRepository,
        IAuthService authService
    ) : IRequestHandler<RecoveryLoginCommand, Result<AuthDto>>
    {
        public async Task<Result<AuthDto>> Handle(RecoveryLoginCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.LoginRequest is null)
            {
                logger.LogWarning("Recovery login request payload was null");
                return Result<AuthDto>.Failure("Invalid request payload.");
            }

            try
            {
                var principal = authService.ValidateTempJwt(request.LoginRequest.TempToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? principal.FindFirst("sub")?.Value;

                var user = await userRepository.GetByIdAsync(Guid.Parse(userId!));
                if (user == null)
                {
                    logger.LogWarning("Recovery login attempt failed. User not found: {UserId}", userId);
                    return Result<AuthDto>.Failure("User not found.");
                }

                await auditLogRepository.AddAsync(new AuditLog(
                    user?.Id ?? Guid.Empty,
                    "RecoveryLoginAttempt",
                    "UserRecoveryCode",
                    null,
                    $"Recovery code attempt: {request.LoginRequest.RecoveryCode}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));


                var recoveryCodeEntity = user.RecoveryCodes
                    .FirstOrDefault(rc => !rc.IsUsed &&
                                          RecoveryCodeHasher.Verify(request.LoginRequest.RecoveryCode, rc.Code));

                if (recoveryCodeEntity is null)
                {
                    logger.LogWarning("Invalid or already used recovery code attempt for user {UserId}", user.Id);
                    await auditLogRepository.AddAsync(new AuditLog(
                        user.Id,
                        "RecoveryLoginFailed",
                        "UserRecoveryCode",
                        null,
                        "Invalid or already used recovery code",
                        request.RequestMetadata.IpAddress,
                        request.RequestMetadata.UserAgent
                    ));
                    return Result<AuthDto>.Failure("Invalid or already used recovery code");
                }

                 
                recoveryCodeEntity.MarkAsUsed();

                
                var token = authService.GenerateToken(new UserDto(
                    user.Id,
                    (string)user.Email,
                    user.FullName,
                    user.DateOfBirth.Age,
                    user.PhoneNumber.Value,
                    user.DateOfBirth.Value,
                    DateTime.UtcNow,
                    user.UserRoles.FirstOrDefault()?.Role.ToString() ?? string.Empty,
                    user.IsEmailVerified,
                    user.IsTwoFactorEnabled, 
                    user.ProfilePictureUrl
                ));

                var refreshToken = authService.GenerateRefreshToken();

                await unitOfWork.BeginTransactionAsync();
                user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
                user.LatestLogin();
                await userRepository.UpdateUserAsync(user);

                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "RecoveryLoginSuccess",
                    "UserRecoveryCode",
                    recoveryCodeEntity.Id,
                    "User logged in successfully using recovery code",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new LoginEvent(user.Id, request.RequestMetadata.IpAddress ?? string.Empty, request.RequestMetadata.UserAgent), cancellationToken);

                logger.LogInformation("User {UserId} logged in successfully using recovery code", user.Id);

                return Result<AuthDto>.Success(new AuthDto(token, refreshToken), "Login Successful");
            }
            catch (SecurityTokenExpiredException)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogWarning("Recovery login failed due to expired temp token for request: {@Request}", request);
                return Result<AuthDto>.Failure("Token expired. Please log in again.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error verifying recovery code for user request: {@Request}", request);
                return Result<AuthDto>.Failure($"Error verifying MFA: {ex.Message}");
            }
        }
    }
}
