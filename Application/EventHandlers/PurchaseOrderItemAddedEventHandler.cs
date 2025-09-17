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
    public class PurchaseOrderItemAddedEventHandler : INotificationHandler<PurchaseOrderItemAddedEvent>
    {
        private readonly ILogger<PurchaseOrderItemAddedEventHandler> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;

        public PurchaseOrderItemAddedEventHandler(
            ILogger<PurchaseOrderItemAddedEventHandler> logger,
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService,
            INotificationRepository notificationRepository)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _emailNotificationService = emailNotificationService;
        }

        public async Task Handle(PurchaseOrderItemAddedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling PurchaseOrderItemAddedEvent for PurchaseOrder {PurchaseOrderId}", notification.PurchaseOrderId);

                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderItemAddedEvent", notification.SupplierId);
                    return;
                }

                var message = $"A new item was added to Purchase Order {notification.orderNumber}. " +
                              $"Product: {notification.ProductName}, " +
                              $"Quantity: {notification.QuantityOrdered}, " +
                              $"Unit Price: ₦{notification.UnitPrice:N2}, " +
                              $"Total: ₦{notification.QuantityOrdered * notification.UnitPrice:N2}";

                string title = "Purchase Order Item Added";

                _logger.LogDebug("Creating in-app notification for Supplier {SupplierId}", supplier.Id);

                var appNotification = new Notification(supplier.UserId, title, message);

                await _notificationRepository.AddAsync(appNotification);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling PurchaseOrderItemAddedEvent for PurchaseOrder {PurchaseOrderId}", notification.PurchaseOrderId);
            }
        }
    }
}
