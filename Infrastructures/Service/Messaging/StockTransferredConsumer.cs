using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Service.Messaging
{
    public class StockTransferredConsumer([FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService
        ,IUserRepository userRepository,
        ILogger<StockTransferredConsumer> logger) : IConsumer<StockTransferredEvent>
    {
        public async Task Consume(ConsumeContext<StockTransferredEvent> context)
        {
            var users = await userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager));
            foreach (var user in users)
            {
                await emailNotificationService.SendEmailAsync(
                    (string)user.Email,
                    "Stock Transfer Notification",
                    $@"
Dear Inventory Manager,

We would like to inform you that a stock transfer has been successfully completed.

Transfer Details:
- Product ID: {context.Message.ProductId}
- Quantity Transferred: {context.Message.Quantity}
- From Warehouse: {context.Message.FromWarehouseName} ({context.Message.FromWarehouseId})
- To Warehouse: {context.Message.ToWarehouseName} ({context.Message.ToWarehouseId})
- Reason: {(string.IsNullOrWhiteSpace(context.Message.Reason) ? "N/A" : context.Message.Reason)}
- Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC

Please verify the transfer in your system and take any necessary follow-up actions.

Regards,  
Inventory System"
                );
            }
            logger.LogInformation("StockTransferredEvent consumed successfully for ProductId: {ProductId}, FromWarehouseId: {FromWarehouseId}, ToWarehouseId: {ToWarehouseId}",
                context.Message.ProductId, context.Message.FromWarehouseId, context.Message.ToWarehouseId);
        }
    }
}
