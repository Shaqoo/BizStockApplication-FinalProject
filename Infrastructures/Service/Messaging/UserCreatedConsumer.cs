using Application.EventHandlers;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using MassTransit.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Service.Messaging
{
    public class UserCreatedConsumer([FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
        ILogger<UserCreatedConsumer> logger) : IConsumer<UserRegisteredEvent>
    {
        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
             var message = context.Message;

            string welcomeMessage = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 30px; border-radius: 10px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);'>
      <h1 style='color: #2d3748;'>Welcome to BizStock, {message.FullName} 👋</h1>
      <p style='font-size: 16px; color: #4a5568;'>Thank you for joining BizStock! We're thrilled to have you on board.</p>
      <p style='font-size: 16px; color: #4a5568;'>Your account has been successfully created, and you now have access to powerful tools to manage your business inventory and operations efficiently.</p>
      <p style='font-size: 16px; color: #4a5568;'>If you ever have questions or need assistance, don’t hesitate to contact our support team. We're here to help!</p>
      <p style='margin-top: 30px; font-size: 14px; color: #718096;'>Cheers,<br/>The BizStock Team</p>
    </div>
  </body>
</html>";
            string emailHtml = $@"
<html>
  <body style='font-family: Arial;'>
    <h2>Welcome to BizStock, {message.FullName}</h2>
    <h2>Secure Your BizStock Account</h2>
    <p>Set up your 2FA by scanning the QR code below in your Google Authenticator app:</p>
    <img src='{message.QrCodeImageUrl}' alt='2FA QR Code' style='max-width: 250px;' />
    <p style=""font-size: 18px; font-weight: bold;"">Or enter this code manually: <strong>{message.ManualEntryKey}</strong></p>
    <p>If you didn’t request this, you can ignore the email.</p>
    <br />
    <p>Thanks,<br/>The BizStock Team</p>
  </body>
</html>";
            logger.LogInformation("Sending welcome emails to {Email}", message.Email);

            await emailNotificationService.SendEmailAsync(message.Email, "Welcome to BizStock", welcomeMessage);
            await emailNotificationService.SendEmailAsync(message.Email, "Secure Your BizStock Account", emailHtml);
        }
        
    }
}
