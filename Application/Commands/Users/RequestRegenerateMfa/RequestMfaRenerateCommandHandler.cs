using Application.Dto;
using Application.Interfaces.Service;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Application.Commands.Users.RequestRegenerateMfa
{
    public class RequestMfaRegenerateCommandHandler(
     IAuthService authService,
     [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
     IMemoryCacheService memoryCacheService,
     ILogger<RequestMfaRegenerateCommandHandler> logger
 ) : IRequestHandler<RequestMfaRenerateCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RequestMfaRenerateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = authService.CurrentUser();
            if (currentUser is null)
            {
                logger.LogWarning("Unauthorized access attempt to request MFA regeneration by an unauthenticated user.");
                return Result<string>.Failure("Unauthorized");
            }

             
            var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);
            int codeInt = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 100_000_000;
            string verificationCode = codeInt.ToString("D8");  

            
            var cacheKey = $"MfaRegenerateRequest_{currentUser.Id}";
            await memoryCacheService.SetAsync(cacheKey, verificationCode, TimeSpan.FromMinutes(10));

            
            var emailTitle = "🔐 MFA Regeneration Verification Code";
            var emailBody = $@"
            <p>Hi {currentUser.Email.Split('@')[0]},</p>
            <p>We received a request to regenerate your Multi-Factor Authentication (MFA) for your account. To ensure this request is secure, please use the following verification code:</p>
            <h2 style='font-family: monospace; font-size: 24px; color: #2563eb;'>{verificationCode}</h2>
            <p>This code is valid for <strong>10 minutes</strong>. Once verified, your old MFA codes will stop working, and you will be able to set up a new MFA device or app.</p>
            <p><strong>Important security tips:</strong></p>
            <ul>
                <li>Do not share this code with anyone.</li>
                <li>If you did not request this, please <a href='mailto:support@yourdomain.com'>contact support immediately</a>.</li>
                <li>After regenerating MFA, store your recovery codes securely.</li>
                <li>Use a trusted authenticator app like Google Authenticator, Authy, or Microsoft Authenticator.</li>
            </ul>
            <p>Thank you for keeping your account secure!</p>
            <p>— The Security Team</p>";

             
            await emailNotificationService.SendEmailAsync(currentUser.Email, emailTitle, emailBody);

            logger.LogInformation("MFA regeneration code sent to {Email}", currentUser.Email);

            return Result<string>.Success("MFA regeneration request received. Please check your email for the verification code.");
        }
    }

}
