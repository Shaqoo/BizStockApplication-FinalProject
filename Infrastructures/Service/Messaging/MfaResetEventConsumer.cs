using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures.Service.Messaging
{
    public class MfaResetEventConsumer : IConsumer<MfaResetEvent>
    {
        private readonly IEmailNotificationService _emailService;

        public MfaResetEventConsumer([FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<MfaResetEvent> context)
        {
            var message = context.Message;

            var body = $@"
Hello {message.FullName},

⚠️ IMPORTANT SECURITY NOTICE ⚠️

Your multi-factor authentication (MFA) configuration has been RESET and NEW recovery codes were generated for your account (**{message.Email}**).  
This action means that your old recovery codes are now INVALID and cannot be used anymore.  

🔒 Why you are receiving this:
- You (or someone with access to your account) explicitly requested a reset of MFA and regeneration of recovery codes.  
- If this was you, please make sure to download, securely store, and protect your new recovery codes immediately.  

🚨 If you DID NOT request this action:
- This may indicate **unauthorized access to your account**.  
- Please change your password immediately and contact our support team at **[Support Email/Phone]**.  
- Consider reviewing your recent account activity for suspicious behavior.  

📅 Action Details:
- User ID: {message.UserId}  
- Email: {message.Email}  
- Time (UTC): {message.ResetAtUtc:u}  

We take your security very seriously. Please do not ignore this message.  

Stay safe,  
The Security Team
";

            await _emailService.SendEmailAsync(
                message.Email,
                "⚠️ Security Alert: MFA Reset and Recovery Codes Regenerated",
                body);
        }

    }

}
