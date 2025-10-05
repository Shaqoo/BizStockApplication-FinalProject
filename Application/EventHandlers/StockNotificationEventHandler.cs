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
    public class StockNotificationEventHandler :
        INotificationHandler<StockReservedEvent>,
        INotificationHandler<StockRestoredEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailNotificationService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;
        private readonly ILogger<StockNotificationEventHandler> _logger;

        public StockNotificationEventHandler(
            INotificationRepository notificationRepository,
            INotifier notifier,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            [FromKeyedServices(EmailNotificationType.Brevo)]IEmailNotificationService emailService,
            ILogger<StockNotificationEventHandler> logger)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _notifier = notifier;
            _logger = logger;
        }

        public async Task Handle(StockReservedEvent notification, CancellationToken cancellationToken)
        {
            var message = $"Stock reserved for Sales Order {notification.SalesOrderId}:\n" +
                          string.Join("\n", notification.Items.Select(i => $"{i.ProductName} - {i.Quantity}"));

            _logger.LogInformation(message);

            var users = await _userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager));

            foreach (var user in users)
            {
                var notif = new Notification(
                    recipientId: user.Id,
                    title: "Stock Reserved",
                    message: message
                );
                await _notificationRepository.AddAsync(notif);

                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = notif.Id,
                    Title = notif.Title,
                    Message = notif.Message,
                    Type = notif.Type,
                    IsRead = notif.IsRead
                });    
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            foreach (var user in users)
            {
                await _emailService.SendEmailAsync((string)user.Email, "Stock Reserved", message);
            }
        }

        public async Task Handle(StockRestoredEvent notification, CancellationToken cancellationToken)
        {
            var message = $"Stock restored for Sales Order {notification.SalesOrderId}:\n" +
                          string.Join("\n", notification.Items.Select(i => $"{i.ProductName} - {i.Quantity}"));

            _logger.LogInformation(message);


            var users = await _userRepository.FindAsync(a => a.UserRoles.Any(a => a.Role == Role.InventoryManager));

            foreach (var user in users)
            {
                var notif = new Notification(
                    recipientId: user.Id,
                    title: "Stock Restored",
                    message: message
                );
                await _notificationRepository.AddAsync(notif);

                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = notif.Id,
                    Title = notif.Title,
                    Message = notif.Message,
                    Type = notif.Type,
                    IsRead = notif.IsRead
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            foreach (var user in users)
            {
                await _emailService.SendEmailAsync((string)user.Email, "Stock Restored", message);
            }
        }
    }

}
