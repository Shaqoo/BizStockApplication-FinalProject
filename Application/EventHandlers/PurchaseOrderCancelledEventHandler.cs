using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using global::Application.Dto;
using global::Application.Interfaces.Repository;
using global::Application.Interfaces.Service;
using global::Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class PurchaseOrderCancelledEventHandler : INotificationHandler<PurchaseOrderCancelledEvent>
    {
        private readonly ILogger<PurchaseOrderCancelledEventHandler> _logger;
        private readonly ISupplierRepository _supplierRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUserRepository _userRepository;

        public PurchaseOrderCancelledEventHandler(
            ILogger<PurchaseOrderCancelledEventHandler> logger,
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

        public async Task Handle(PurchaseOrderCancelledEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderCancelledEvent", notification.SupplierId);
                    return;
                }

                _logger.LogInformation(
                    "Purchase Order {PurchaseOrderId} (OrderNumber: {OrderNumber}) was cancelled. Reason: {Reason}",
                    notification.PurchaseOrderId,
                    notification.OrderNumber,
                    notification.Reason ?? "No reason provided"
                );

                string title = "Purchase Order Cancelled";
                string message = $"Purchase Order {notification.OrderNumber} has been cancelled. " +
                                 $"Reason: {notification.Reason ?? "N/A"}";

                var managers = await _userRepository.FindAsync(u => u.UserRoles.Any(a => a.Role == Role.InventoryManager));
                foreach (var item in managers)
                {
                    var managersNotification = new Notification(item.Id, title, message,"warning");
                    await _notificationRepository.AddAsync(managersNotification);

                    await _notifier.SendNotificationAsync(item.Id, new NotificationDto
                    {
                        Id = managersNotification.Id,
                        Title = title,
                        Message = message,
                        IsRead = managersNotification.IsRead,
                        Type = managersNotification.Type
                    });

                    await _emailNotificationService.SendEmailAsync((string)item.Email, title, message);
                }

                var appNotification = new Notification(supplier.UserId, title, message, "success");
                await _notificationRepository.AddAsync(appNotification);

                await _unitOfWork.SaveChangesAsync();

                await _notifier.SendNotificationAsync(supplier.UserId, new NotificationDto
                {
                    Id = appNotification.Id,
                    Title = title,
                    Message = message,
                    IsRead = appNotification.IsRead,
                    Type = appNotification.Type
                });

                await _emailNotificationService.SendEmailAsync((string)supplier.Email, title, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling PurchaseOrderCancelledEvent for PO {PurchaseOrderId}",
                    notification.PurchaseOrderId);
            }
        }
    }
}
