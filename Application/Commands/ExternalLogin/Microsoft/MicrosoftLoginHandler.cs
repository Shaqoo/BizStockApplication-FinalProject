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

namespace Application.Commands.ExternalLogin.Microsoft
{
    public class MicrosoftLoginHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAuditLogRepository logRepository,
        IMediator mediator,
        ILogger<MicrosoftLoginHandler> logger,
        IAuthService authService) : IRequestHandler<MicrosoftLoginCommand, Result<AuthDto>>
    {
        public async Task<Result<AuthDto>> Handle(MicrosoftLoginCommand request, CancellationToken cancellationToken)
        {
            if (request.dto.AccessToken is null)
                return Result<AuthDto>.Failure("Invalid request payload.");

            var accessToken = request.dto.AccessToken;
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me", cancellationToken);

            if(!response.IsSuccessStatusCode)
            {
                logger.LogError("Microsoft user request failed. Status code: {StatusCode}", response.StatusCode);
                return Result<AuthDto>.Failure("Unable to retrieve Microsoft user.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var msUser = JsonSerializer.Deserialize<MicrosoftUserDto>(json);

            if(string.IsNullOrWhiteSpace(msUser?.UserPrincipalName) || string.IsNullOrWhiteSpace(msUser?.Mail))
                return Result<AuthDto>.Failure("No valid email found in Microsoft account.");

            var email = msUser.Mail ?? msUser.UserPrincipalName;


            var user = await userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                await logRepository.AddAsync(new AuditLog(
                    Guid.Empty,
                    "Microsoft_LOGIN_NO_USER",
                    "ExternalLogin",
                    null,
                    $"Login attempt failed — no user found for email: {email}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                logger.LogWarning("Microsoft login failed — no user found with email: {Email}", email);
                return Result<AuthDto>.Failure("No account associated with this Microsoft email.");
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

            await logRepository.AddAsync(new AuditLog(
                user.Id,
                "Microsoft_LOGIN_SUCCESS",
                "ExternalLogin",
                user.Id,
                "User logged in successfully via Microsoft.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

        logger.LogInformation("Microsoft login successful for user {UserId}", user.Id);

        return Result<AuthDto>.Success(new AuthDto(token, refreshToken), "Login successful.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Microsoft login failed due to unexpected error.");
            await unitOfWork.RollbackTransactionAsync();
            return Result<AuthDto>.Failure("Unexpected error during Microsoft login.");
        }
}
    }
}
