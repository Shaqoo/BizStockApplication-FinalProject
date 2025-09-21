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
    public class PurchaseOrderRejectedEventHandler : INotificationHandler<PurchaseOrderRejectedEvent>
    {
        private readonly ILogger<PurchaseOrderRejectedEventHandler> _logger;
        private readonly ISupplierRepository _supplierRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUserRepository _userRepository;

        public PurchaseOrderRejectedEventHandler(
            ILogger<PurchaseOrderRejectedEventHandler> logger,
            ISupplierRepository supplierRepository,
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            IUserRepository userRepository,
            [FromKeyedServices(EmailNotificationType.Mailjet)] IEmailNotificationService emailNotificationService)
        {
            _logger = logger;
            _supplierRepository = supplierRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _userRepository = userRepository;
            _emailNotificationService = emailNotificationService;
        }

        public async Task Handle(PurchaseOrderRejectedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderRejectedEvent", notification.SupplierId);
                    return;
                }

                _logger.LogInformation(
                    "Purchase Order {PurchaseOrderId} (OrderNumber: {OrderNumber}) was rejected. Reason: {Reason}",
                    notification.PurchaseOrderId,
                    notification.OrderNumber,
                    notification.Reason ?? "No reason provided"
                );

                string title = "Purchase Order Rejected";
                string message = $"Purchase Order {notification.OrderNumber} has been rejected. " +
                                 $"Reason: {notification.Reason ?? "N/A"}";

                var managers = await _userRepository.FindAsync(u => u.UserRoles.Any(r => r.Role == Role.InventoryManager));
                foreach (var manager in managers)
                {
                    var managerNotification = new Notification(manager.Id, title, message,"warning");
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

                var supplierNotification = new Notification(supplier.UserId, title, message, "success");
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
                _logger.LogError(ex,
                    "Error handling PurchaseOrderRejectedEvent for PO {PurchaseOrderId}",
                    notification.PurchaseOrderId);
            }
        }
    }
}
