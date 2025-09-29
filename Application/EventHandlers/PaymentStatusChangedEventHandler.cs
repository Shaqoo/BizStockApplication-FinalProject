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
    public class PaymentStatusChangedEventHandler : INotificationHandler<PaymentStatusChangedEvent>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ILogger<PaymentStatusChangedEventHandler> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotifier _notifier;

        public PaymentStatusChangedEventHandler(
            ICustomerRepository customerRepository,
            INotificationRepository notificationRepository,
            [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailNotificationService,
            IUserRepository userRepository,
            ILogger<PaymentStatusChangedEventHandler> logger,
            INotifier notifier,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _notificationRepository = notificationRepository;
            _emailNotificationService = emailNotificationService;
            _userRepository = userRepository;
            _logger = logger;
            _notifier = notifier;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PaymentStatusChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(notification.CustomerId);
                if (customer is null)
                {
                    _logger.LogWarning("Customer {CustomerId} not found for payment {PaymentId}",
                        notification.CustomerId, notification.PaymentId);
                    return;
                }
                var user = await _userRepository.GetByEmailAsync((string)customer.Email);
                if (user is null)
                {
                    _logger.LogWarning("User {CustomerId} not found for payment {PaymentId}",
                        notification.CustomerId, notification.PaymentId);
                    return;
                }


                var inAppMessage = notification.Status == PaymentStatus.Completed
                    ? $"Payment of {notification.Amount:C} successful."
                    : $"Payment of {notification.Amount:C} failed.";

                var inAppNotification = new Notification(user.Id, "Payment Update",inAppMessage);
         
                await _notificationRepository.AddAsync(inAppNotification);

                await _unitOfWork.SaveChangesAsync();
                await _notifier.SendNotificationAsync(user.Id, new NotificationDto
                {
                    Id = inAppNotification.Id,
                    IsRead = inAppNotification.IsRead,
                    Message = inAppNotification.Message,
                    Title = inAppNotification.Title,
                    Type = inAppNotification.Type,
                    Timestamp = DateTime.UtcNow
                });

                var emailMessage = $@"
Hello {customer.FullName},

Your payment has been processed with the following details:

- Amount: {notification.Amount:C}
- Status: {notification.Status}
- Payment Reference: {notification.Reference}

{(notification.Status == PaymentStatus.Completed ? "Thank you for your payment." : "Please try again or contact support.")}

Best regards,
Your Company Name
";

                await _emailNotificationService.SendEmailAsync(
                    to: (string)customer.Email,
                    subject: "Payment Status Update",
                    body: emailMessage
                );

                _logger.LogInformation("Payment notification sent to Customer {CustomerId} for Payment {PaymentId}",
                    customer.Id, notification.PaymentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending payment notification for Payment {PaymentId}", notification.PaymentId);
            }
        }
    }

}
