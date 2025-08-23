using Application.Dto.RequestModels;
using Domain.DomainEvents;
using Domain.Entities;
using global::Application.Dto;
using global::Application.Interfaces.Repository;
using global::Application.Interfaces.Service;
using global::Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Application.Commands.ExternalLogin.GitHub
{
    namespace Application.Commands.ExternalLogin.GitHub
    {
        public class GitHubLoginHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            IMediator mediator,
            ILogger<GitHubLoginHandler> logger)
            : IRequestHandler<GitHubLoginCommand, Result<AuthDto>>
        {
            public async Task<Result<AuthDto>> Handle(GitHubLoginCommand request, CancellationToken cancellationToken)
            {
                if (request.Dto.AccessToken is null)
                    return Result<AuthDto>.Failure("Invalid request payload.");

                var accessToken = request.Dto.AccessToken;
                var httpClient = new HttpClient();

                 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("BizStockApp");

                try
                {
                     
                    var userResponse = await httpClient.GetAsync("https://api.github.com/user", cancellationToken);
                    if (!userResponse.IsSuccessStatusCode)
                    {
                        logger.LogError("GitHub user request failed. Status code: {StatusCode}", userResponse.StatusCode);
                        return Result<AuthDto>.Failure("Unable to retrieve GitHub user.");
                    }

                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    var gitHubUser = JsonSerializer.Deserialize<GitHubUserDto>(userJson);

                     
                    var emailResponse = await httpClient.GetAsync("https://api.github.com/user/emails", cancellationToken);
                    if (!emailResponse.IsSuccessStatusCode)
                    {
                        logger.LogError("GitHub email request failed. Status code: {StatusCode}", emailResponse.StatusCode);
                        return Result<AuthDto>.Failure("Unable to retrieve GitHub email.");
                    }

                    var emailJson = await emailResponse.Content.ReadAsStringAsync();

                    var emails = JsonSerializer.Deserialize<List<GitHubEmailDto>>(emailJson);

                    var primaryEmail = emails?.FirstOrDefault(e => e.Primary && e.Verified)?.Email;

                    if (string.IsNullOrWhiteSpace(primaryEmail))
                    {
                        logger.LogWarning("No verified primary email found for GitHub user.");
                        return Result<AuthDto>.Failure("GitHub account must have a verified primary email.");
                    }

                    var user = await userRepository.GetByEmailAsync(primaryEmail);
                    if (user == null)
                    {
                        await auditLogRepository.AddAsync(new AuditLog(
                            Guid.Empty,
                            "GITHUB_LOGIN_NO_USER",
                            "ExternalLogin",
                            null,
                            $"Login attempt failed — no user found for email: {primaryEmail}",
                            request.RequestMetadata.IpAddress,
                            request.RequestMetadata.UserAgent
                        ));

                        logger.LogWarning("GitHub login failed — no user found with email: {Email}", primaryEmail);
                        return Result<AuthDto>.Failure("No account associated with this GitHub email.");
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
                        user.IsEmailVerified,
                        user.IsTwoFactorEnabled, 
                        user.ProfilePictureUrl
                    ));

                    var refreshToken = authService.GenerateRefreshToken();

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

                    await auditLogRepository.AddAsync(new AuditLog(
                        user.Id,
                        "GITHUB_LOGIN_SUCCESS",
                        "ExternalLogin",
                        user.Id,
                        "User logged in successfully via GitHub.",
                        request.RequestMetadata.IpAddress,
                        request.RequestMetadata.UserAgent
                    ));

                    logger.LogInformation("GitHub login successful for user {UserId}", user.Id);

                    return Result<AuthDto>.Success(new AuthDto(token, refreshToken), "Login successful.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GitHub login failed due to unexpected error.");
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<AuthDto>.Failure("Unexpected error during GitHub login.");
                }
            }
        }
    }

}
