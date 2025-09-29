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
    public class PaymentInitializedEventHandler : INotificationHandler<PaymentInitializedEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailNotificationService _emailService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<PaymentInitializedEventHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly INotifier _notifier;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentInitializedEventHandler(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailService,
            ICustomerRepository customerRepository,
            ILogger<PaymentInitializedEventHandler> logger,
            INotifier notifier,
            IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _customerRepository = customerRepository;
            _logger = logger;
            _userRepository = userRepository;
            _notifier = notifier;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PaymentInitializedEvent notification, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);
            if (customer is null)
                return;
            var user = await _userRepository.GetByEmailAsync((string)customer.Email);
            if (user is null) return;

            var inAppNotification = new Notification(user.Id, "Payment Initialized", $"Payment of {notification.Amount:C} has been initialized.");
            await _notificationRepository.AddAsync(inAppNotification);
            await _unitOfWork.SaveChangesAsync();
            await _notifier.SendNotificationAsync(user.Id,new NotificationDto 
            {
                Id = inAppNotification.Id,
                IsRead = inAppNotification.IsRead,
                Message = inAppNotification.Message,
                Title = inAppNotification.Title,
                Type = inAppNotification.Type,
                Timestamp = DateTime.UtcNow
            });
             
            var emailBody = $@"
Hello {customer.FullName},

Your payment of {notification.Amount:C} has been initialized.
Reference: {notification.Reference}

Please complete the payment to finalize the transaction.

Thank you.";

            await _emailService.SendEmailAsync(customer.Email.Value, "Payment Initialized", emailBody);

            _logger.LogInformation("Payment initialization notification sent to Customer {CustomerId}", customer.Id);
        }
    }

}
