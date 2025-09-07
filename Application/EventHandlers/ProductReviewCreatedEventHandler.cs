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
    public class ProductReviewCreatedEventHandler : INotificationHandler<ProductReviewCreatedEvent>
    {
        private readonly INotifier _notifier;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductReviewCreatedEventHandler(
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

        public async Task Handle(ProductReviewCreatedEvent notification, CancellationToken cancellationToken)
        {
            var admins = await _userRepository.FindAsync(u =>
    u.UserRoles.Any(ur => ur.Role == Role.Admin || ur.Role == Role.Manager));


            await _unitOfWork.BeginTransactionAsync();
            foreach (var admin in admins)
            {
                var notificationEntity = new Notification(
                    recipientId: admin.Id,
                    title: "Product Review Pending",
                    message: $"A review has been submitted for product '{notification.ProductName}' and requires your approval.",
                    type: "info",
                    linkUrl: $"/products/{notification.ProductId}/reviews"
                );

                await _notificationRepository.AddAsync(notificationEntity);

                await _notifier.SendNotificationAsync(admin.Id, new NotificationDto
                {
                    Id = notificationEntity.Id,
                    Title = notificationEntity.Title,
                    Message = notificationEntity.Message,
                    Type = notificationEntity.Type,
                });
            }

            await _unitOfWork.CommitTransactionAsync();
        }
    }

}
