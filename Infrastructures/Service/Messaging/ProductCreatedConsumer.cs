using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class ProductCreatedConsumer(
    IUserRepository userRepository,
    [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService notificationService,
    ILogger<ProductCreatedConsumer> logger)
    : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var users = await userRepository.FindAsync(u =>
            u.UserRoles.Any(a => a.Role == Role.InventoryManager) ||
            u.UserRoles.Any(a => a.Role == Role.Manager) ||
            u.UserRoles.Any(a => a.Role == Role.Admin));

        var emailUsers = users
            .Where(u => !string.IsNullOrWhiteSpace(u.Email?.Value))
            .Select(u => new
            {
                Email = u.Email!.Value,
                Name = u.FullName
            })
            .ToList();

        var subject = "New Product Created";
        var body = $"A new product has been created: {context.Message.Name} with SKU: {context.Message.SKU}";

        const int batchSize = 10;
        const int delayBetweenBatchesMs = 1000;

        var batches = emailUsers.Chunk(batchSize);  

        foreach (var batch in batches)
        {
            var sendTasks = batch.Select(async user =>
            {
                try
                {
                    await notificationService.SendEmailAsync(user.Email, subject, body);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to send email to {Email}", user.Email);
                }
            });

            await Task.WhenAll(sendTasks);
            await Task.Delay(delayBetweenBatchesMs);  
        }
    }
}
