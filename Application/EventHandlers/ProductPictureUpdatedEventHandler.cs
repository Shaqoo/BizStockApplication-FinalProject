using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EventHandlers
{
    public class ProductPictureUpdatedEventHandler : INotificationHandler<ProductPictureUpdatedEvent>
    {
        private readonly INotifier _notifier;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductPictureUpdatedEventHandler(INotifier notifier, INotificationRepository notificationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _notifier = notifier;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductPictureUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var inventoryManagers = await _userRepository.FindAsync(u => u.UserRoles.Any(a => a.Role == Role.InventoryManager)
            || u.HasRole(Role.Manager));

            await _unitOfWork.BeginTransactionAsync();
            foreach (var manager in inventoryManagers)
            {
                var entity = new Notification(
                    manager.Id,
                    "Product Picture Updated",
                    $"Picture of product '{notification.ProductName}' was updated.",
                    "info",
                    $"/products/{notification.ProductId}"
                );

                await _notificationRepository.AddAsync(entity);
                await _notifier.SendNotificationAsync(manager.Id, new NotificationDto
                {
                    Title = entity.Title,
                    Message = entity.Message,
                    Type = entity.Type,
                });
            }

            await _unitOfWork.CommitTransactionAsync();
        }
    }

}
