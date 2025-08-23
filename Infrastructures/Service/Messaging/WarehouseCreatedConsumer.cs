using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Service.Messaging
{
    public class WarehouseCreatedConsumer(
        [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService service,
        ILogger<WarehouseCreatedConsumer> logger,
        IUserRepository userRepository)
        : IConsumer<WarehouseCreatedEvent>
    {
        public async Task Consume(ConsumeContext<WarehouseCreatedEvent> context)
        {
            var users = await userRepository.FindAsync(a =>
                a.UserRoles.Any(a => a.Role == Role.InventoryManager) ||
                a.UserRoles.Any(a => a.Role == Role.Admin) ||
                a.UserRoles.Any(a => a.Role == Role.Manager));

            var emailUsers = users
                .Where(u => !string.IsNullOrWhiteSpace(u.Email?.Value))
                .Select(u => new
                {
                    Email = u.Email!.Value,
                    Name = u.FullName
                })
                .ToList();

            const int batchSize = 10;
            const int delayBetweenBatchesMs = 1000;

            var htmlMessage = $@"<html>
                <body>
                    <h2>New Warehouse Created</h2>
                    <p>A new warehouse has been created with the following details:</p>
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
                        await service.SendEmailAsync(user.Email, "New Warehouse Created", htmlMessage);
                        logger.LogInformation("Notification sent to {Email}", user.Email);
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
