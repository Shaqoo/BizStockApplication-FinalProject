using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Infrastructures.Service.Messaging
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IEmailNotificationService _emailService;

        public OrderCreatedEventConsumer(
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailService)
        {
            _emailService = emailService;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var order = context.Message;

            try
            {
                var sb = new StringBuilder();

                sb.AppendLine($"<h2>Hi {order.CustomerName},</h2>");
                sb.AppendLine($"<p>Thank you for your order <strong>{order.OrderNumber}</strong>.</p>");
                sb.AppendLine($"<p>Your order reference is <strong>{order.DeliveryReference}</strong>.</p>");

                sb.AppendLine("<h3>Order Summary</h3>");
                sb.AppendLine("<table style='width:100%; border-collapse:collapse;'>");
                sb.AppendLine("<thead>");
                sb.AppendLine("<tr style='background-color:#3B82F6;'>");
                sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Product</th>");
                sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>SKU</th>");
                sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Quantity</th>");
                sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Unit Price (₦)</th>");
                sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Total (₦)</th>");
                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");
                sb.AppendLine("<tbody>");

                foreach (var product in order.Products)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'><img src='{product.ImageUrl}' alt='{product.Name}' width='50' style='width:50px; max-width:50px; height:auto; display:block; margin-right:10px;' /> {product.Name}</td>");
                    sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'>{product.Sku}</td>");
                    sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'>{product.Quantity}</td>");
                    sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'>{product.UnitPrice:N0}</td>");
                    sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'>{product.LineTotal:N0}</td>");
                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                sb.AppendLine("<br/>");
                sb.AppendLine("<h3>Order Totals</h3>");
                sb.AppendLine("<table style='width:50%; border-collapse:collapse;'>");
                sb.AppendLine("<tr><td style='padding:8px;'>Subtotal:</td><td style='padding:8px;'>₦" + order.SubTotal.ToString("N0") + "</td></tr>");
                sb.AppendLine("<tr><td style='padding:8px;'>Delivery Cost:</td><td style='padding:8px;'>₦" + order.DeliveryCost.ToString("N0") + "</td></tr>");
                sb.AppendLine("<tr style='font-weight:bold;'><td style='padding:8px;'>Total:</td><td style='padding:8px;'>₦" + order.Total.ToString("N0") + "</td></tr>");
                sb.AppendLine("</table>");

                sb.AppendLine("<br/>");
                sb.AppendLine("<p>We’ll notify you when your order is out for delivery.</p>");
                sb.AppendLine("<p style='margin-top:20px;'>Thanks for shopping with us 🎉</p>");
                sb.AppendLine("<p>The Customer Service Team</p>");

                
                await _emailService.SendEmailAsync(
                    to: order.CustomerEmail,
                    subject: $"Order Confirmation - {order.OrderNumber}",
                    body: sb.ToString()
                );
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"[OrderCreatedEventConsumer] Failed to send order email: {ex.Message}");
            }
        }
    }
}
