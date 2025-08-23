using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Application.Commands.Users.RequestPasswordChange
{
    public class RequestPasswordChangeHandler(
     [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotification,
     IMemoryCacheService cacheService,
     IUserRepository userRepository,
     IAuditLogRepository logRepository
 ) : IRequestHandler<RequestPasswordCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RequestPasswordCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await userRepository.GetByEmailAsync(request.Request.Email);
            if (userEntity == null)
            {
                return Result<string>.Failure("User not found.");
            }
            var token = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            var cacheKey = $"PasswordChangeToken:{userEntity.Id}";
            await cacheService.SetAsync(cacheKey, token, TimeSpan.FromMinutes(10));

            var emailContent = $@"
<html>
  <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
    <div style=""max-width: 600px; margin: auto; background-color: white; padding: 30px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);"">
      <h2 style=""color: #333;"">🔐 Password Change Request</h2>
      <p>Hello <strong>{userEntity!.FullName}</strong>,</p>
      <p>You have requested to change your password. Please use the verification code below to proceed:</p>
      <div style=""font-size: 24px; font-weight: bold; padding: 15px; background-color: #f0f0f0; text-align: center; border-radius: 5px; letter-spacing: 3px;"">
        {token}
      </div>
      <p>This code is valid for <strong>10 minutes</strong>.</p>
      <p>If you did not initiate this request, you can safely ignore this email.</p>
      <hr />
      <p style=""font-size: 12px; color: #888;"">Thanks,<br/>The BizStock Team</p>
    </div>
  </body>
</html>
";

            try
            {
                await emailNotification.SendEmailAsync((string)userEntity.Email, "🔐 Password Change Request", emailContent);

                await logRepository.AddAsync(new AuditLog(
                    userEntity.Id,
                    "RequestPasswordChange",
                    "User",
                    null,
                    $"Password change token requested for user {(string)userEntity.Email}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                return Result<string>.Success("A password change token has been sent to your email.");
            }
            catch (Exception ex)
            {
                await logRepository.AddAsync(new AuditLog(
                    userEntity?.Id ?? Guid.Empty,
                    "RequestPasswordChangeFailed",
                    "System",
                    null,
                    $"Failed to send password change token. Error: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                return Result<string>.Failure("Failed to send password change token. Please try again later.");
            }
        }
    }

}
