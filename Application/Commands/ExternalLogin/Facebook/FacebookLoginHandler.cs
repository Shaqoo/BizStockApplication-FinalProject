using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Application.Commands.ExternalLogin.Facebook
{
    public class FacebookLoginHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IAuditLogRepository auditLogRepository,
        IMediator mediator,
        ILogger<FacebookLoginHandler> logger)
        : IRequestHandler<FacebookLoginCommand, Result<AuthDto>>
    {
        public async Task<Result<AuthDto>> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
        {
            if (request?.Dto?.AccessToken is null)
                return Result<AuthDto>.Failure("Invalid request payload.");

            var accessToken = request.Dto.AccessToken;
            var httpClient = new HttpClient();

            var userInfoUrl = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to retrieve Facebook user info. Status code: {StatusCode}", response.StatusCode);
                return Result<AuthDto>.Failure("Invalid or expired Facebook token.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<FacebookUserDto>(content);

            if (userInfo is null || string.IsNullOrWhiteSpace(userInfo.Email))
            {
                logger.LogWarning("Facebook user info is missing or email is empty.");
                return Result<AuthDto>.Failure("Could not retrieve valid user info from Facebook.");
            }

            var user = await userRepository.GetByEmailAsync(userInfo.Email);
            if (user is null)
            {
                await auditLogRepository.AddAsync(new AuditLog(
                    Guid.Empty,
                    "FACEBOOK_LOGIN_NO_USER",
                    "ExternalLogin",
                    null,
                    $"Login attempt failed — no user found for email: {userInfo.Email}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                logger.LogWarning("Facebook login failed — no user found with email: {Email}", userInfo.Email);
                return Result<AuthDto>.Failure("No account associated with this Facebook email.");
            }

            if (user.IsDeleted)
                return Result<AuthDto>.Failure("User account is deactivated.");

            if (user.IsLockedOut)
                return Result<AuthDto>.Failure("Account is locked. Please try again later.");

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
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ), cancellationToken);

                logger.LogInformation("Facebook login successful for user {UserId}", user.Id);

                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "FACEBOOK_LOGIN_SUCCESS",
                    "ExternalLogin",
                    user.Id,
                    "User logged in successfully via Facebook.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                return Result<AuthDto>.Success(new AuthDto(token, refreshToken), "Login successful.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Facebook login failed for user {UserId}.", user.Id);
                return Result<AuthDto>.Failure("Unexpected error occurred during login.");
            }
        }
    }
}
