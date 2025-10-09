using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures.Service.Messaging
{
    public class OrderStatusChangedConsumer(
        [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService)
        : IConsumer<OrderStatusChangedEvent>
    {
        public async Task Consume(ConsumeContext<OrderStatusChangedEvent> context)
        {
            var notification = context.Message;

            if (string.IsNullOrEmpty(notification.CustomerEmail))
                return;

            string subject = $"Update on Your Order #{notification.OrderId.ToString().Substring(0, 8).ToUpper()}";

            
            string trackingUrl = $"https://bizstock.com/orders/track/{notification.OrderId}";
            string companyColor = "#1E40AF";  

            string body = $@"
            <div style='font-family: Arial, sans-serif; color: #333; background-color: #f9fafb; padding: 40px 0;'>
              <div style='max-width: 600px; margin: auto; background: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.08);'>
                <div style='background: {companyColor}; padding: 20px; text-align: center;'>
                  <h1 style='color: #fff; margin: 0; font-size: 24px;'>BizStock Order Update</h1>
                </div>

                <div style='padding: 30px;'>
                  <h2 style='color:{companyColor};'>Hi {notification.CustomerName ?? "Valued Customer"},</h2>
                  <p style='font-size: 16px;'>
                    We wanted to let you know that the status of your order has been updated.
                  </p>

                  <div style='margin: 25px 0; padding: 20px; background:#F3F4F6; border-radius:10px;'>
                    <p style='margin:0;'><strong>Order ID:</strong> {notification.OrderId}</p>
                    {(string.IsNullOrEmpty(notification.TrackingNumber) ? "" : $"<p><strong>Tracking Number:</strong> {notification.TrackingNumber}</p>")}
                    <p><strong>Previous Status:</strong> {notification.OldStatus}</p>
                    <p><strong>New Status:</strong> <span style='color:{companyColor}; font-weight:600;'>{notification.NewStatus}</span></p>
                    <p><strong>Updated At:</strong> {notification.ChangedAt:dddd, dd MMM yyyy HH:mm tt}</p>
                    {(string.IsNullOrEmpty(notification.Message) ? "" : $"<p style='margin-top:10px;'>{notification.Message}</p>")}
                  </div>

                  <div style='text-align:center; margin: 30px 0;'>
                    <a href='{trackingUrl}' target='_blank' style='background:{companyColor}; color:#fff; text-decoration:none; padding:12px 24px; border-radius:8px; font-weight:600;'>
                      Track My Order
                    </a>
                  </div>

                  <p style='font-size: 15px; color: #374151;'>
                    You can check your delivery progress anytime using the button above.
                  </p>
                  <p style='margin-top: 20px; font-size: 15px;'>
                    If you have any questions or concerns, feel free to contact our support team at
                    <a href='mailto:support@bizstock.com' style='color:{companyColor}; text-decoration:none;'>support@bizstock.com</a>.
                  </p>

                  <p style='margin-top: 30px;'>Thank you for shopping with us!<br/><strong>– The BizStock Team</strong></p>
                </div>

                <div style='background:#F3F4F6; text-align:center; padding:20px; font-size:12px; color:#6B7280;'>
                  <p>This email was sent automatically. Please do not reply directly to it.</p>
                  <p>&copy; {DateTime.UtcNow.Year} BizStock Inc. All rights reserved.</p>
                </div>
              </div>
            </div>";

            await emailNotificationService.SendEmailAsync(
                notification.CustomerEmail,
                subject,
                body);
        }
    }
}
