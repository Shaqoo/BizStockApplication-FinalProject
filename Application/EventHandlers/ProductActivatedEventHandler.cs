using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.EventHandlers
{
    public class ProductActivatedEventHandler : INotificationHandler<ProductActivatedEvent>
    {
        private readonly INotifier _notifier;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductActivatedEventHandler(
            INotifier notifier,
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _notifier = notifier;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductActivatedEvent notification, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(u => u.UserRoles.Any(a => a.Role == Role.Manager)
            || u.UserRoles.Any(a => a.Role == Role.InventoryManager));

            await _unitOfWork.BeginTransactionAsync();
            foreach (var user in users)
            {
                var notificationEntity = new Notification(
                    recipientId: user.Id,
                    title: "Product Activated",
                    message: $"Product '{notification.ProductName}' has been activated by a user.",
                    type: "info",
                    linkUrl: $"/products/{notification.ProductId}"
                );

                await _notificationRepository.AddAsync(notificationEntity);
                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = notificationEntity.Id,
                    Title = notificationEntity.Title,
                    Message = notificationEntity.Message,
                    Type = notificationEntity.Type
                });
            }
            await _unitOfWork.CommitTransactionAsync();
        }
    }

}
