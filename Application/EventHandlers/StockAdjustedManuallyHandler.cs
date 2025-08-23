using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class StockAdjustedManuallyHandler(INotificationRepository notificationRepository,
     IUnitOfWork unitOfWork,
     IUserRepository userRepository,
     INotifier notifier,
     ILogger<StockAdjustedManuallyHandler> logger) : INotificationHandler<StockAdjustedManuallyEvent>
    {
        public async Task Handle(StockAdjustedManuallyEvent notification, CancellationToken cancellationToken)
        {
            var users = await userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager)
            || a.UserRoles.Any(a => a.Role == Role.Manager));
            if (!users.Any())
                return;

            await unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var user in users)
                {
                    var message = $@"
Stock Adjustment Notice:
- Product: {notification.ProductName} ({notification.ProductId})
- Warehouse: {notification.WarehouseName} ({notification.WarehouseId})
- Quantity Changed: {notification.QuantityChanged:+#;-#;0}
- Final Quantity: {notification.FinalQuantity}
- Reason: {(string.IsNullOrWhiteSpace(notification.Reason) ? "N/A" : notification.Reason)}
- Performed By: {notification.PerformedBy}
- Date: {notification.AdjustedAt:yyyy-MM-dd HH:mm:ss} UTC";

                    var notificationDto = new NotificationDto
                    {
                        ThreadId = user.Id,
                        Message = message,
                        Type = "info",
                        Timestamp = DateTime.UtcNow,
                        Title = "Manual Stock Adjustment",
                    };

                    await notifier.SendNotificationAsync(user.Id, notificationDto);
                    await notificationRepository.AddAsync(new Notification(user.Id, notificationDto.Title, notificationDto.Message));
                }

                await unitOfWork.CommitTransactionAsync();

                logger.LogInformation("StockAdjustedManuallyEvent handled successfully for ProductId: {ProductId}, WarehouseId: {WarehouseId}",
                    notification.ProductId, notification.WarehouseId);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error handling StockAdjustedManuallyEvent for ProductId: {ProductId}, WarehouseId: {WarehouseId}",
                    notification.ProductId, notification.WarehouseId);
                throw;
            }
        }

    }
}
