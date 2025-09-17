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
    public class PurchaseOrderItemRemovedEventHandler : INotificationHandler<PurchaseOrderItemRemovedEvent>
    {
        private readonly ILogger<PurchaseOrderItemRemovedEventHandler> _logger;
        private readonly INotificationRepository _notificationService;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;
        private readonly IEmailNotificationService _emailNotificationService;

        public PurchaseOrderItemRemovedEventHandler(
            ILogger<PurchaseOrderItemRemovedEventHandler> logger,
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

        public async Task Handle(PurchaseOrderItemRemovedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var supplier = await _supplierRepository.GetByIdAsync(notification.SupplierId);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier {SupplierId} not found when handling PurchaseOrderItemRemovedEvent", notification.SupplierId);
                    return;
                }

                _logger.LogInformation(
                    "Purchase Order Item {ItemId} removed from Purchase Order {PurchaseOrderId} (OrderNumber: {OrderNumber})",
                    notification.ItemId,
                    notification.PurchaseOrderId,
                    notification.orderNumber
                );

                string title = "Purchase Order Item Removed";
                string message =
                    $"An item (ItemId: {notification.ItemId}) was removed from Purchase Order {notification.PurchaseOrderId} " +
                    $"(OrderNumber: {notification.orderNumber}).";

                var appNotification = new Notification(supplier.UserId, title, message);

                await _notificationService.AddAsync(appNotification);
                await _unitOfWork.SaveChangesAsync();

                await _notifier.SendNotificationAsync(supplier.UserId, new NotificationDto
                {
                    Id = appNotification.Id,
                    Title = title,
                    Message = message,
                    IsRead = appNotification.IsRead,
                    Type = appNotification.Type,
                });

                await _emailNotificationService.SendEmailAsync((string)supplier.Email, title, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling PurchaseOrderItemRemovedEvent for PurchaseOrderId {PurchaseOrderId}, ItemId {ItemId}",
                    notification.PurchaseOrderId,
                    notification.ItemId);
            }
        }
    }
}
