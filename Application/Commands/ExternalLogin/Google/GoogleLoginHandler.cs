using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ExternalLogin.Google
{
    public class GoogleLoginHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    IHttpContextAccessor httpContextAccessor,
    IMediator mediator,
    ILogger<GoogleLoginHandler> logger
) : IRequestHandler<GoogleLoginCommand, Result<AuthDto>>
    {
        public async Task<Result<AuthDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Google login process for access token: {Token}", request.Dto.AccessToken);

            GoogleJsonWebSignature.Payload? payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.Dto.AccessToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to validate Google access token.");
                return Result<AuthDto>.Failure("Invalid Google token.");
            }

            if (payload == null || string.IsNullOrWhiteSpace(payload.Email))
            {
                logger.LogWarning("Google token payload is null or email is empty.");

                return Result<AuthDto>.Failure("Invalid Google token.");
            }

            var user = await userRepository.GetByEmailAsync(payload.Email);
            if (user == null)
            {
                await auditLogRepository.AddAsync(new AuditLog(
                     Guid.Empty,
                    "GOOGLE_LOGIN_ATTEMPT_NO_USER",
                    "ExternalLogin",
                    null,
                    $"Login attempt with Google account but no user found for email: {payload.Email}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                logger.LogWarning("Login failed. No user found with email: {Email}", payload.Email);
                return Result<AuthDto>.Failure("No account associated with this Google account.");
            }

            if (user.IsLockedOut)
            {
                await mediator.Publish(new AccountLockedEvent(user.FullName, (string)user.Email, request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent), cancellationToken);
            }

            if (user.IsDeleted)
            {
                logger.LogWarning("Login attempt for deactivated user: {UserId}", user.Id);
                return Result<AuthDto>.Failure("User account is deactivated.");
            }

            var token = authService.GenerateToken(new UserDto(
                user.Id,
                (string)user.Email,
                user.FullName,
                user.DateOfBirth.Age, 
                user.PhoneNumber.Value,
                user.DateOfBirth.Value,
                DateTime.UtcNow,
                user.UserRoles.FirstOrDefault()?.Role.ToString() ?? string.Empty,
                user.Gender.ToString(),
                user.IsEmailVerified,
                user.IsTwoFactorEnabled,
                user.ProfilePictureUrl
            ));

            var refreshToken = authService.GenerateRefreshToken();

            try
            {
                await unitOfWork.BeginTransactionAsync();

                user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
                user.LatestLogin();
                user.ResetLoginAttempts();

                await userRepository.UpdateUserAsync(user);
                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new LoginEvent(
                    user.Id,
                    request.RequestMetadata.IpAddress!,
                    request.RequestMetadata.UserAgent
                ), cancellationToken);

                logger.LogInformation("Google login successful for user {UserId}", user.Id);

                await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "Google Login ATTEMPT_SUCCESS",
                "User",
                user.Id,
                "Successful login attempt.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

                httpContextAccessor?.HttpContext?.Response.ClearRefreshToken();
                httpContextAccessor?.HttpContext?.Response.SetRefreshToken(refreshToken);

                return Result<AuthDto>.Success(new AuthDto(token, refreshToken), "Login Successful");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Login failed for user {UserId} due to an error.", user.Id);
                return Result<AuthDto>.Failure($"Unexpected error during login: {ex.Message}");
            }
        }
    }

}
