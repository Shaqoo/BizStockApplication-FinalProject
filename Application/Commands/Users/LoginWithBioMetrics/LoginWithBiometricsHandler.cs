using Application.Configurations;
using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Users.LoginWithBioMetrics
{
    public class LoginWithBiometricsHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IFidoCredentialService fido2Service,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator,
        IAuthService authService,
        IAuditLogRepository auditLogRepository) : IRequestHandler<LoginWithBiometricsCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(LoginWithBiometricsCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginDto is null)
                return Result<object>.Failure("Invalid login request");

            var assertion = request.LoginDto.ToFido2Assertion();

            Guid userId;
            try
            {
                await unitOfWork.BeginTransactionAsync();
                userId = await fido2Service.VerifyAssertionAsync(assertion);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<object>.Failure("Fingerprint verification failed: " + ex.Message);
            }

            var user = await userRepository.GetByIdAsync(userId);
            if (user is null || user.IsDeleted || user.IsLockedOut)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<object>.Failure("User not allowed or account locked.");
            }


            user.ResetLoginAttempts();
          

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "Fingerprint Login",
                "User",
                user.Id,
                "Successful login attempt.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

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

            await mediator.Publish(new LoginEvent(user.Id, request.RequestMetadata.IpAddress!, request.RequestMetadata.UserAgent), cancellationToken);

            httpContextAccessor?.HttpContext?.Response.ClearRefreshToken();
            httpContextAccessor?.HttpContext?.Response.SetRefreshToken(refreshToken);

            return Result<object>.Success(new AuthDto(token, refreshToken), "Login Successful");

        }
    }
}
