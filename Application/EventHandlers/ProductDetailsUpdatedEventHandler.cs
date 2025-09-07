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
    public class ProductDetailsUpdatedEventHandler : INotificationHandler<ProductDetailsUpdatedEvent>
    {
        private readonly INotifier _notifier;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDetailsUpdatedEventHandler(INotifier notifier, INotificationRepository notificationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _notifier = notifier;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ProductDetailsUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(u => u.UserRoles.Any(a => a.Role == Role.Manager) 
            || u.UserRoles.Any(a => a.Role == Role.InventoryManager));

            await _unitOfWork.BeginTransactionAsync();
            foreach (var user in users)
            {
                var entity = new Notification(
                    user.Id,
                    "Product Details Updated",
                    $"Details of product '{notification.ProductName}' were updated.",
                    "info",
                    $"/products/{notification.ProductId}"
                );

                await _notificationRepository.AddAsync(entity);
                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Message = entity.Message,
                    Type = entity.Type
                });
            }

            await _unitOfWork.CommitTransactionAsync();
        }
    }

}
