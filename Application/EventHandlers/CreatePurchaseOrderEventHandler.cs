using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class CreatePurchaseOrderEventHandler(
    ISupplierRepository supplierRepository,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork,
    INotifier notifier,
    ILogger<CreatePurchaseOrderEventHandler> logger,
    [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService
) : INotificationHandler<CreatePurchaseOrderEvent>
    {
        public async Task Handle(CreatePurchaseOrderEvent notification, CancellationToken cancellationToken)
        {
            var supplier = await supplierRepository.GetByIdAsync(notification.SupplierId);
            if (supplier == null)
            {
                logger.LogWarning("Supplier {SupplierId} not found when handling CreatePurchaseOrderEvent", notification.SupplierId);
                return;
            }

            var breakdown = string.Join(Environment.NewLine, notification.Items.Select(i =>
                $"- {i.ProductName} (x{i.QuantityOrdered}) @ ₦{i.UnitPrice:N2} = ₦{(i.QuantityOrdered * i.UnitPrice):N2}"
            ));

            var message =
                $"📦 New Purchase Order {notification.OrderNumber}" + Environment.NewLine +
                $"Expected Delivery: {notification.ExpectedDeliveryDate?.ToString("yyyy-MM-dd") ?? "N/A"}" + Environment.NewLine +
                $"Discount: ₦{notification.Discount:N2}" + Environment.NewLine +
                $"Tax: ₦{notification.Tax:N2}" + Environment.NewLine +
                $"--- Items ---" + Environment.NewLine +
                breakdown + Environment.NewLine +
                $"-----------------" + Environment.NewLine +
                $"Total: ₦{(notification.Items.Sum(i => i.QuantityOrdered * i.UnitPrice) - notification.Discount + notification.Tax):N2}";

            var inAppNotification = new Notification(supplier.UserId, $"New purchase order {notification.OrderNumber} has been created.", message, "info");
            await notificationRepository.AddAsync(inAppNotification);
            await unitOfWork.SaveChangesAsync();

            await notifier.SendNotificationAsync(supplier.UserId, new NotificationDto
            {
                Title = $"New Purchase Order {notification.OrderNumber}",
                Message = message,
                Type = "info",
                Id = inAppNotification.Id,
                IsRead = inAppNotification.IsRead,
            });

            await emailNotificationService.SendEmailAsync((string)supplier.Email, $"New purchase order {notification.OrderNumber} has been created.", message);
        }
    }
}
