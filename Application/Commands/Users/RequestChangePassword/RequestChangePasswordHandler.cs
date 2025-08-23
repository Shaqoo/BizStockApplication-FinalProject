using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.RequestChangePassword
{
    public class RequestChangePasswordHandler(IAuthService authService,
        IUserRepository userRepository,
        ILogger<RequestChangePasswordHandler> logger,
        IAuditLogRepository auditLogRepository) : IRequestHandler<RequestChangePasswordCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RequestChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser == null)
            {
                logger.LogWarning("Unauthorized access attempt by user with ID {UserId}", request.requestMetadata.UserAgent);
                return Result<string>.Failure("You are not authorized to perform this action.");
            }
            var user = await userRepository.GetByIdAsync(currentUser.Id);
            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found", currentUser.Id);
                return Result<string>.Failure("User not found.");
            }
            var hashedPassword = PasswordHasher.HashPassword(request.requestChange.password,user.HashSalt);

            if (user.Password != hashedPassword)
            {
                logger.LogWarning("Password change request failed for user with ID {UserId}: Password mismatch", currentUser.Id);
                return Result<string>.Failure("The provided password does not match the current password.");
            }

            await auditLogRepository.AddAsync(new AuditLog(currentUser.Id,"RequestPasswordChange",
                nameof(User),user.Id, $"Password change request validated for user '{user.Email}' (ID: {user.Id}).",
                request.requestMetadata.IpAddress,
                request.requestMetadata.UserAgent));
            return Result<string>.Success("Password change request is valid. You can proceed with the password change.");
        }
    }
}
