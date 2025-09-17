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
    public class PurchaseOrderUpdatedEventHandler : INotificationHandler<PurchaseOrderUpdatedEvent>
    {
        private readonly ILogger<PurchaseOrderUpdatedEventHandler> _logger;
        private readonly INotificationRepository _notificationService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;

        public PurchaseOrderUpdatedEventHandler(
            ILogger<PurchaseOrderUpdatedEventHandler> logger,
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
            INotificationRepository notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _emailNotificationService = emailNotificationService;
        }

        public async Task Handle(PurchaseOrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling PurchaseOrderUpdatedEvent for PurchaseOrderId {PurchaseOrderId}", notification.PurchaseOrderId);

            var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderUpdatedEvent", notification.SupplierId);
                return;
            }

            _logger.LogInformation(
                "Purchase Order {PurchaseOrderId} updated. Notes: {Notes}, Discount: {Discount}, Tax: {Tax}",
                notification.PurchaseOrderId,
                notification.Notes ?? "N/A",
                notification.Discount,
                notification.Tax
            );

            var message = $"Purchase Order {notification.PurchaseOrderId} has been updated. " +
                          $"OrderNumber {notification.orderNumber} " +
                          $"Notes: {notification.Notes ?? "No notes"}, " +
                          $"Discount: #{notification.Discount:N2}, Tax: #{notification.Tax:N2}";

            string title = "Purchase Order Updated";

            _logger.LogDebug("Creating in-app notification for Supplier {SupplierId}", supplier.Id);

            var appNotification = new Notification(supplier.UserId, title, message);

            await _notificationService.AddAsync(appNotification);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Saved notification {NotificationId} for Supplier {SupplierId}", appNotification.Id, supplier.Id);

            _logger.LogDebug("Sending real-time notification to Supplier {SupplierId}", supplier.Id);

            await _notifier.SendNotificationAsync(supplier.UserId, new NotificationDto
            {
                Id = appNotification.Id,
                Title = title,
                Message = message,
                IsRead = appNotification.IsRead,
                Type = appNotification.Type,
            });

            _logger.LogInformation("Real-time notification sent to Supplier {SupplierId}", supplier.Id);

            _logger.LogDebug("Sending email notification to Supplier {SupplierId} at {Email}", supplier.Id, supplier.Email);

            await _emailNotificationService.SendEmailAsync((string)supplier.Email, title, message);

            _logger.LogInformation("Email notification sent to Supplier {SupplierId}", supplier.Id);
        }
    }
}
