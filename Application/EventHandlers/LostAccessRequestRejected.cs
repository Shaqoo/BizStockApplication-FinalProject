using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class LostAcessRequestRejectedHandler
        : INotificationHandler<LostAccessRequestRejectedEvent>
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ILogger<LostAcessRequestRejectedHandler> _logger;

        public LostAcessRequestRejectedHandler(
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
            ILogger<LostAcessRequestRejectedHandler> logger)
        {
            _emailNotificationService = emailNotificationService;
            _logger = logger;
        }

        public async Task Handle(LostAccessRequestRejectedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var subject = "Lost Access Request - Rejected";
                var body = $@"
                    Hello,

                    Your lost access request submitted on {notification.CreatedAt:MMMM dd, yyyy} 
                    has been reviewed and unfortunately it was <b>rejected</b> by our support team.

                    Notes from the administrator:
                    {notification.Notes ?? "No additional notes provided."}

                    If you still cannot access your account, please try again using a different recovery option 
                    (alternate email, phone, or security questions), or contact support for further assistance.

                    Regards,
                    The Support Team
                ";

                await _emailNotificationService.SendEmailAsync(
                    notification.UserIdentifier,
                    subject,
                    body
                );

                _logger.LogInformation(
                    "Rejected LostAccessRequest notification sent for RequestId {RequestId} to {Recipient}",
                    notification.RequestId,
                    notification.UserIdentifier
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error sending rejection email for LostAccessRequest {RequestId}",
                    notification.RequestId);
            }
        }
    }
}
