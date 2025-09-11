using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Application.Extensions;
using Application.Commands.Carts.LinkToUser;

namespace Application.EventHandlers
{
    public class LoginEventHandler(
        IUserRepository userRepository,
        IMediator mediator,
        ILogger<LoginEventHandler> logger,
        [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailService,
        IHttpContextAccessor httpContextAccessor) : INotificationHandler<LoginEvent>
    {
        public async Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(notification.UserId);
            if (user == null) return;

            var subject = "🛡️ MFA Login Detected - BizStock";
            var time = DateTime.UtcNow.ToString("f");

            string emailBody = $@"
        <html>
        <body style='font-family: Arial, sans-serif; padding: 20px;'>
            <h2 style='color: #2d3748;'>MFA Login Alert</h2>
            <p>Hello {user.FullName},</p>
            <p>Your BizStock account was accessed with MFA at:</p>
            <ul>
                <li><strong>Time:</strong> {time} (UTC)</li>
                <li><strong>IP Address:</strong> {notification.IpAddress}</li>
                <li><strong>Device:</strong> {notification.DeviceInfo}</li>
            </ul>
            <p>If this was you, no action is needed. If you didn't authorize this login, please secure your account immediately.</p>
            <hr />
            <p>Need help? Contact support:</p>
            <p>Email: <a href='mailto:ShakirullahOhio@gmail.com'>ShakirullahOhio@gmail.com</a><br/>
            Phone: <a href='tel:+2348109094694'>+234 810 909 4694</a></p>
            <br/>
            <p>Stay safe,<br/>The BizStock Team</p>
        </body>
        </html>";

            await emailService.SendEmailAsync((string)user.Email,subject, emailBody);

            logger.LogInformation("Login Mail Messages Sent");

            if (user.UserRoles.Any(a => a.Role == Role.Customer))
            {

                var sessionId = httpContextAccessor.HttpContext?.GetOrCreateCartSessionId();
                if (sessionId != null)
                {
                    logger.LogInformation("Cart session ID: {SessionId}", sessionId);
                    var command = new LinkCartToUserCommand(notification.UserId, sessionId);
                    var linked = await mediator.Send(command, cancellationToken);
                    if (linked.IsSuccess)
                    {
                        logger.LogInformation("Cart linked to user successfully.");
                    }
                    else
                    {
                        logger.LogWarning("Failed to link cart to user: {ErrorMessage}", linked.Message);
                    }
                }
                else
                {
                    logger.LogWarning("Failed to retrieve cart session ID.");
                }
            }
        }
    }

}
