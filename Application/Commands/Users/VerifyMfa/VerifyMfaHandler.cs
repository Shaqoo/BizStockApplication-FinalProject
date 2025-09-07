using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application.Commands.Users.VerifyMfa
{
    public class VerifyMfaHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMfaService mfaService,
        IHttpContextAccessor httpContextAccessor,
        IAuthService authService) : IRequestHandler<VerifyMfaCommand, Result<AuthDto>>
    {
        public async Task<Result<AuthDto>> Handle(VerifyMfaCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Request is null)
            {
                return Result<AuthDto>.Failure("Invalid request payload.");
            }

            try
            {
                var principal = authService.ValidateTempJwt(request.Request.TempToken);
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? principal.FindFirst("sub")?.Value;

                var user = await userRepository.GetByIdAsync(Guid.Parse(userId!));
                if (user == null)
                {
                    return Result<AuthDto>.Failure("User not found.");
                }
                Console.WriteLine(string.Join(' ',user.UserRoles));

                var isValid = await mfaService.VerifySecretAsync(user, request.Request.MfaCode);
                if (!isValid)
                {
                    return Result<AuthDto>.Failure("Invalid TOTP code.");
                }

                var token = authService.GenerateToken(new UserDto(user.Id,
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

                await unitOfWork.BeginTransactionAsync();
                user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
                user.LatestLogin();
                await userRepository.UpdateUserAsync(user);
                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new LoginEvent(user.Id,request.RequestMetadata.IpAddress!,request.RequestMetadata.UserAgent),cancellationToken);

                httpContextAccessor?.HttpContext?.Response.ClearRefreshToken();
                httpContextAccessor?.HttpContext?.Response.SetRefreshToken(refreshToken);

                return Result<AuthDto>.Success(new AuthDto(token,refreshToken),"Login Successful");
            }
            catch (SecurityTokenExpiredException)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<AuthDto>.Failure("Token expired. Please log in again.");
            }

            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<AuthDto>.Failure($"Error verifying MFA: {ex.Message}");
            }
        }
    }
}
