using Application.Dto;
using Application.Interfaces.Service;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Commands.Users.SendEmailVerificationToken
{
    public class SendEmailVerificationTokenCommandHandler
        : IRequestHandler<SendEmailVerificationTokenCommand, Result<string>>
    {
        private readonly IMemoryCacheService _cache;
        private readonly IEmailNotificationService _emailService;

        public SendEmailVerificationTokenCommandHandler(
            IMemoryCacheService cache,
            [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailService)
        {
            _cache = cache;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(SendEmailVerificationTokenCommand request, CancellationToken cancellationToken)
        {
            var token = Guid.NewGuid().ToString("N");

            var cacheKey = $"email_verification:{request.Email}";
            await _cache.SetAsync(cacheKey,token, TimeSpan.FromMinutes(15));

            var verificationLink = $"https://localhost:7124/verify?userMail={request.Email}&token={token}";

            var message = $@"
Hello,

Thank you for registering with us.  
To complete your registration, please verify your email address.  

Your verification token is: **{token}**

Alternatively, you can click the link below to verify directly:  
{verificationLink}

If you did not register on our platform, please ignore this email.  

Best regards,  
Your App Team
";

            // 4. Send email
            await _emailService.SendEmailAsync(
                request.Email,
                "Verify your email address",
                message
            );

            return Result<string>.Success("Verification email sent successfully.");
        }
    }

}
