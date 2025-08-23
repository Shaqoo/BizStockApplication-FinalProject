using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.EventHandlers
{
    public class AccountLockedEventHandler(
     [FromKeyedServices(EmailNotificationType.Brevo)]IEmailNotificationService emailNotificationService)
     : INotificationHandler<AccountLockedEvent>
    {
        public async Task Handle(AccountLockedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.FullName;
            var ip = notification.IpAddress;
            var device = notification.DeviceInfo;

            var subject = "🚫 Your BizStock Account Has Been Locked";
            var body = $"""
        <h2>🚫 Account Locked for Security Reasons</h2>

        <p>Dear {user},</p>

        <p>We detected multiple failed login attempts on your BizStock account. As a result, we've temporarily locked your account to protect it from unauthorized access.</p>

        <h4>🔍 Details of the last attempt:</h4>
        <ul>
          <li><strong>IP Address:</strong> {ip}</li>
          <li><strong>Device:</strong> {device}</li>
          <li><strong>Time:</strong> {DateTime.UtcNow:f} UTC</li>
        </ul>

        <p>If this was you, please try again after 10 minutes or reset your password.</p>

        <p>If this wasn't you or if you need urgent access, contact our support team:</p>

        <ul>
          <li><strong>Phone:</strong> <a href="tel:+2348109094694">+2348109094694</a></li>
          <li><strong>Email:</strong> <a href="mailto:ShakirullahOhio@gmail.com">ShakirullahOhio@gmail.com</a></li>
        </ul>

        <p>We’re here to help keep your account safe.</p>

        <p>– The BizStock Security Team</p>
        """;

            await emailNotificationService.SendEmailAsync(notification.Email, subject, body);
        }
    }
}
