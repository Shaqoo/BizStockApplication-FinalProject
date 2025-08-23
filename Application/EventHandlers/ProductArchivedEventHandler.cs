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
    public class ProductArchivedEventHandler : INotificationHandler<ProductArchivedEvent>
    {
        private readonly INotifier _notifier;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductArchivedEventHandler(
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

        public async Task Handle(ProductArchivedEvent notification, CancellationToken cancellationToken)
        {
           
            var users = await _userRepository.FindAsync(u => u.UserRoles.Any(a => a.Role == Role.Manager) || u.UserRoles.Any(a => a.Role == Role.InventoryManager));
            await _unitOfWork.BeginTransactionAsync();

            foreach (var user in users)
            {
                var notificationEntity = new Notification(
                    recipientId: user.Id,
                    title: "Product Archived",
                    message: $"Product '{notification.ProductName}' has been archived.",
                    type: "warning",
                    linkUrl: $"/products/{notification.ProductId}"
                );

                await _notificationRepository.AddAsync(notificationEntity);

                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Title = notificationEntity.Title,
                    Message = notificationEntity.Message,
                    Type = notificationEntity.Type
                });
            }

            await _unitOfWork.CommitTransactionAsync();
        }
    }

}
