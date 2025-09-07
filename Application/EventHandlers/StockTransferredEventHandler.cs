using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class StockTransferredEventHandler(
     INotificationRepository notificationRepository,
     IUnitOfWork unitOfWork,
     IUserRepository userRepository,
     INotifier notifier,
     ILogger<StockTransferredEventHandler> logger
 ) : INotificationHandler<StockTransferredEvent>
    {
        public async Task Handle(StockTransferredEvent notification, CancellationToken cancellationToken)
        {
            var users = await userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager));
            if (!users.Any())
                return;

            await unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var user in users)
                {
                    var message = $"Stock transferred: {notification.Quantity} units of product {notification.ProductId} from warehouse {notification.FromWarehouseId} to warehouse {notification.ToWarehouseId}.";

                    var notificationDto = new NotificationDto
                    {
                        ThreadId = user.Id,
                        Message = message,
                        Type = "info",
                        Timestamp = DateTime.UtcNow,
                        Title = "Stock Transfer Notification",
                    };

                    var userNotification = new Notification(user.Id, notificationDto.Title, notificationDto.Message);
                    notificationDto.Id = userNotification.Id;
                    await notifier.SendNotificationAsync(user.Id, notificationDto);
                    await notificationRepository.AddAsync(userNotification);
                }
                await unitOfWork.CommitTransactionAsync();
                logger.LogInformation("StockTransferredEvent handled successfully for ProductId: {ProductId}, FromWarehouseId: {FromWarehouseId}, ToWarehouseId: {ToWarehouseId}",
                    notification.ProductId, notification.FromWarehouseId, notification.ToWarehouseId);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error handling StockTransferredEvent for ProductId: {ProductId}, FromWarehouseId: {FromWarehouseId}, ToWarehouseId: {ToWarehouseId}",
                    notification.ProductId, notification.FromWarehouseId, notification.ToWarehouseId);
                throw;
            }
        }
    }

}
