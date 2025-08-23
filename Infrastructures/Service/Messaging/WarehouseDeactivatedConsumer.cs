using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Service.Messaging
{
    public class WarehouseDeactivatedConsumer([FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService service,
        ILogger<WarehouseCreatedConsumer> logger,
        IUserRepository userRepository) : IConsumer<WarehouseDeactivatedEvent>
    {
        public async Task Consume(ConsumeContext<WarehouseDeactivatedEvent> context)
        {
            var users = await userRepository.FindAsync(a =>
                a.UserRoles.Any(a => a.Role == Role.InventoryManager) ||
                a.UserRoles.Any(a => a.Role == Role.Admin) ||
                a.UserRoles.Any(a => a.Role == Role.Manager));

            var emailUsers = users.Where(a => !string.IsNullOrWhiteSpace(a.Email?.Value)).Select(a => new
            {
                Email = a.Email!.Value,
            }).ToList();

            const int batchSize = 10;
            const int delayBetweenBatchesMs = 1000;

            string message = $@"<html>
                <body>
                    <h2>Warehouse Deactivated</h2>
                    <p>The following warehouse has been deactivated:</p>
                    <p><strong>Name:</strong> {context.Message.Name}</p>
                    <p><strong>Location:</strong> {context.Message.Location}</p>
                </body>
            </html>";

            var batches = emailUsers.Chunk(batchSize);

            foreach (var batch in batches)
            {
                var tasks = batch.Select(async user =>
                {
                    try
                    {
                        await service.SendEmailAsync(user.Email, "Warehouse Deactivated",message);
                        logger.LogInformation("Notification sent to {Email}", user.Email);
                        logger.LogInformation("Deactivation email sent to {Email} for warehouse {WarehouseName}",
                    user.Email, context.Message.Name);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to send email to {Email}", user.Email);
                    }
                });

                await Task.WhenAll(tasks);
                await Task.Delay(delayBetweenBatchesMs);
            }
        }

    }
}
