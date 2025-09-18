using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class PurchaseOrderConfirmedEventHandler : INotificationHandler<PurchaseOrderConfirmedEvent>
    {
        private readonly ILogger<PurchaseOrderConfirmedEventHandler> _logger;
        private readonly ISupplierRepository _supplierRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUserRepository _userRepository;

        public PurchaseOrderConfirmedEventHandler(
            ILogger<PurchaseOrderConfirmedEventHandler> logger,
            IUserRepository userRepository,
            ISupplierRepository supplierRepository,
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService)
        {
            _logger = logger;
            _supplierRepository = supplierRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _emailNotificationService = emailNotificationService;
        }

        public async Task Handle(PurchaseOrderConfirmedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning(
                        "Supplier {SupplierId} not found when handling PurchaseOrderConfirmedEvent",
                        notification.SupplierId
                    );
                    return;
                }

                _logger.LogInformation(
                    "Purchase Order {PurchaseOrderId} (OrderNumber: {OrderNumber}) was confirmed. Comment: {Comment}",
                    notification.PurchaseOrderId,
                    notification.OrderNumber,
                    notification.Comment ?? "No comment provided"
                );

                string title = "Purchase Order Confirmed";
                string message = $"Purchase Order {notification.OrderNumber} has been confirmed. " +
                                 $"Comment: {notification.Comment ?? "N/A"}";

                var managers = await _userRepository.FindAsync(
                    u => u.UserRoles.Any(r => r.Role == Role.InventoryManager)
                );
                foreach (var manager in managers)
                {
                    var managerNotification = new Notification(manager.Id, title, message);
                    await _notificationRepository.AddAsync(managerNotification);

                    await _notifier.SendNotificationAsync(manager.Id, new NotificationDto
                    {
                        Id = managerNotification.Id,
                        Title = title,
                        Message = message,
                        IsRead = managerNotification.IsRead,
                        Type = managerNotification.Type
                    });

                    await _emailNotificationService.SendEmailAsync((string)manager.Email, title, message);
                }

                var supplierNotification = new Notification(supplier.UserId, title, message);
                await _notificationRepository.AddAsync(supplierNotification);
                await _unitOfWork.SaveChangesAsync();

                await _notifier.SendNotificationAsync(supplier.UserId, new NotificationDto
                {
                    Id = supplierNotification.Id,
                    Title = title,
                    Message = message,
                    IsRead = supplierNotification.IsRead,
                    Type = supplierNotification.Type
                });

                await _emailNotificationService.SendEmailAsync((string)supplier.Email, title, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error handling PurchaseOrderConfirmedEvent for PO {PurchaseOrderId}",
                    notification.PurchaseOrderId
                );
            }
        }
    }
}
