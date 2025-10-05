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
    public class RefundProcessedEventHandler : INotificationHandler<RefundProcessedEvent>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailNotificationService _emailService;
        private readonly ILogger<RefundProcessedEventHandler> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotifier _notifier;
        private readonly IUnitOfWork _unitOfWork;

        public RefundProcessedEventHandler(
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailService,
            IUnitOfWork unitOfWork,
            INotifier notifier,
            ILogger<RefundProcessedEventHandler> logger,
            INotificationRepository notificationRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _notifier = notifier;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }

        public async Task Handle(RefundProcessedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(notification.customerId);
                if (customer == null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found", notification.customerId);
                    return;
                }

                var user = await _userRepository.GetByEmailAsync((string)customer.Email);
                if (user == null)
                {
                    _logger.LogWarning("User with email {Email} not found", (string)customer.Email);
                    return;
                }

                string subject = $"Refund {notification.Status} for Order {notification.SalesOrderNumber}";
                string emailContent = $@"
                    Dear {customer.FullName},<br/><br/>
                    Your refund has been processed.<br/><br/>
                    <strong>Order Number:</strong> {notification.SalesOrderNumber}<br/>
                    <strong>Refund Amount:</strong> ₦{notification.Amount:N2}<br/>
                    <strong>Payment Method:</strong> {notification.PaymentMethod}<br/>
                    <strong>Refund Reference:</strong> {notification.RefundReference}<br/>
                    <strong>Status:</strong> {notification.Status}<br/>
                    <strong>Reason:</strong> {notification.Reason}<br/><br/>
                    The refund should reflect in your account shortly.<br/><br/>
                    Thank you for shopping with us.<br/>
                    <strong>BizStock Team</strong>
                ";

                await _emailService.SendEmailAsync(
                    to: (string)customer.Email,
                    subject: subject,
                    body: emailContent
                );

               
                string message = $"Your refund of ₦{notification.Amount:N2} for order {notification.SalesOrderNumber} has been {notification.Status.ToLower()}.";

                var inAppNotification = new Notification(
                    recipientId: user.Id,
                    title: "Refund Processed",
                    message: message,
                    type: "info",
                    linkUrl: $"/orders/{notification.SalesOrderId}"
                );

                await _notificationRepository.AddAsync(inAppNotification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Refund processed notification sent for Refund {RefundId}", notification.RefundId);
                await _notifier.SendNotificationAsync(
                    userId: user.Id,
                    new NotificationDto
                    {
                      Title = inAppNotification.Title,
                      Id = inAppNotification.Id,
                      Message = inAppNotification.Message,
                      Type = inAppNotification.Type,
                      IsRead = inAppNotification.IsRead
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to handle RefundProcessedEvent for Refund {RefundId}", notification.RefundId);
            }
        }
    }
}
