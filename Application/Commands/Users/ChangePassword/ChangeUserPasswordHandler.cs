using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.ChangePassword
{
    public class ChangeUserPasswordHandler(IAuthService authService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAuditLogRepository logRepository,
        ILogger<ChangeUserPasswordHandler> logger) : IRequestHandler<ChangeUserPasswordCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                logger.LogWarning("Unauthorized access attempt by user with ID {UserId}", request.RequestMetadata.UserAgent);
                return Result<string>.Failure("You are not authorized to perform this action.");
            }
            var existingUser = await userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
            {
                logger.LogWarning("User with ID {UserId} not found", user.Id);
                return Result<string>.Failure("User not found.");
            }
            var hashedPassword = PasswordHasher.HashPassword(request.ChangePasswordRequest.newPassword, existingUser.HashSalt);
            existingUser.ChangePassword(hashedPassword,existingUser.HashSalt);
            await unitOfWork.BeginTransactionAsync();
            try
            {
                await userRepository.UpdateUserAsync(existingUser);
                await logRepository.AddAsync(new AuditLog(existingUser.Id, "ChangePassword",
                    nameof(User), existingUser.Id, $"Password changed successfully for user '{existingUser.Email}' (ID: {existingUser.Id}).",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent));
                await unitOfWork.CommitTransactionAsync();
                logger.LogInformation("Password changed successfully for user with ID {UserId}", existingUser.Id);
                return Result<string>.Success("Password changed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error changing password for user with ID {UserId}", existingUser.Id);
                await unitOfWork.RollbackTransactionAsync();
                return Result<string>.Failure("An error occurred while changing the password. Please try again later.");
            }

        }
    }
}
