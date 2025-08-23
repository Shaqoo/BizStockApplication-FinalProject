using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Service.Messaging
{
    public class StockAdjustedConsumer(
    [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
    IUserRepository userRepository,
    ILogger<StockAdjustedConsumer> logger) : IConsumer<StockAdjustedManuallyEvent>
    {
        public async Task Consume(ConsumeContext<StockAdjustedManuallyEvent> context)
        {
            var @event = context.Message;
            var users = await userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager) 
            || a.UserRoles.Any(a => a.Role == Role.Manager));

            if (!users.Any())
            {
                logger.LogWarning("No Inventory Manager found to notify.");
                return;
            }

            foreach (var user in users)
            {
                var subject = "Stock Adjustment Notification";

                var message = $@"
Dear {user.FullName},

A manual stock adjustment was performed.

Details:
- Product: {@event.ProductName}
- Warehouse: {@event.WarehouseName}
- Quantity Changed: {@event.QuantityChanged}
- Final Quantity: {@event.FinalQuantity}
- Reason: {@event.Reason}
- Adjusted By: {@event.PerformedBy}
- Date: {@event.AdjustedAt:dd MMM yyyy, HH:mm} UTC

Please review the inventory records for further details.

Best regards,
Inventory System
";

                await emailNotificationService.SendEmailAsync(
                    (string)user.Email,
                    subject,
                    message
                );
            }

            logger.LogInformation("Stock adjustment notification sent to {Count} inventory managers for Product: {ProductName}", users.Count(), @event.ProductName);
        }
    }

}
