using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
   
    public class LostAcessRequestApprovedHandler
        : INotificationHandler<LostAccessRequestApprovedEvent>
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ILogger<LostAcessRequestApprovedHandler> _logger;

        public LostAcessRequestApprovedHandler(
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
            ILogger<LostAcessRequestApprovedHandler> logger)
        {
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        public async Task Handle(LostAccessRequestApprovedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var subject = "Your Lost Access Request Has Been Approved";
                var body = $@"
            <p>Hello {notification.Fullname},</p>
            <p>Your lost access request submitted on <b>{notification.CreatedAt:dd MMM yyyy}</b> 
            has been <b>approved</b> by our admin team.</p>

            <p><b>Details:</b></p>
            <ul>
                <li><b>Status:</b> {notification.Status}</li>
                <li><b>Admin Notes:</b> {notification.Notes ?? "No notes provided"}</li>
            </ul>

            <p>Two-Factor Authentication (MFA) has been <b>disabled</b> on your account to allow you regain access.
            For your security, we recommend re-enabling MFA once you have logged in successfully.</p>

            <p>If you did not request this change, please contact support immediately.</p>

            <p>Best regards,<br/>Security Team</p>";

                await _emailNotificationService.SendEmailAsync(
                    notification.UserIdentifier,
                    subject,
                    body
                );

                _logger.LogInformation(
                    "Approval email sent to {Email} for LostAccessRequest {RequestId}.",
                    notification.UserIdentifier, notification.RequestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send approval email for LostAccessRequest {RequestId}", notification.RequestId);
            }
        }
    }

}
