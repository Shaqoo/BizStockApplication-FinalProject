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
    public class PurchaseOrderItemsReceivedEventHandler : INotificationHandler<PurchaseOrderItemsReceivedEvent>
    {
        private readonly ILogger<PurchaseOrderItemsReceivedEventHandler> _logger;
        private readonly ISupplierRepository _supplierRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IUserRepository _userRepository;

        public PurchaseOrderItemsReceivedEventHandler(
            ILogger<PurchaseOrderItemsReceivedEventHandler> logger,
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

        public async Task Handle(PurchaseOrderItemsReceivedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderItemsReceivedEvent", notification.SupplierId);
                    return;
                }

                var itemsDescription = string.Join(", ",
                    notification.Items.Select(i => $"ItemId {i.PurchaseOrderItemId}: Qty {i.QuantityReceived}"));

                _logger.LogInformation(
                    "Purchase Order {PurchaseOrderId} (OrderNumber: {OrderNumber}) items received. Items: {Items}",
                    notification.PurchaseOrderId,
                    notification.OrderNumber,
                    itemsDescription
                );

                string title = "Purchase Order Items Received";
                string message = $"Items have been received for Purchase Order {notification.OrderNumber}. " +
                                 $"Details: {itemsDescription}";

             
                var managers = await _userRepository.FindAsync(u => u.UserRoles.Any(r => r.Role == Role.InventoryManager));
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
                _logger.LogError(ex,
                    "Error handling PurchaseOrderItemsReceivedEvent for PO {PurchaseOrderId}",
                    notification.PurchaseOrderId);
            }
        }
    }
}
