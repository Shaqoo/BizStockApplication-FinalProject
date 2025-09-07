using Application.Commands.Wishlists.CreateWishlist;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.EventHandlers
{
    public class UserRegisteredEventHandler(
        INotifier notifier,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IAuditLogRepository auditLogRepository,
        ILogger<UserRegisteredEventHandler> logger,
        IHttpContextAccessor httpContextAccessor) : INotificationHandler<UserRegisteredEvent>
    {
        public async Task Handle(UserRegisteredEvent @event, CancellationToken cancellationToken)
        {
            string ip = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
            string userAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"]!;

            logger.LogInformation("Handling UserRegisteredEvent for user ID {UserId}", @event.UserId);

            string notificationText = "Your BizStock account was successfully created. Welcome aboard!";
            var notification = new Notification(@event.UserId, "Welcome to BizStock", notificationText, "info");
            var dto = new NotificationDto
            {
                Id = notification.Id,
                Title = "Welcome to BizStock",
                Message = notificationText,
                Type = "info",
            };

            try
            {
                await unitOfWork.BeginTransactionAsync();
                await notificationRepository.AddAsync(notification);
                await unitOfWork.CommitTransactionAsync();

                await notifier.SendNotificationAsync(@event.UserId,dto);

                await auditLogRepository.AddAsync(new AuditLog(
                    userId: @event.UserId,
                    action: "User Registered",
                    entityName: "User",
                    entityId: @event.UserId,
                    details: $"User registered successfully with email {@event.Email}",
                    ip: ip,
                    userAgent: userAgent
                ));

                var wishlist = new CreateWishlistCommand(@event.UserId);
                var result = await mediator.Send(wishlist);
                if(!result.IsSuccess)
                {
                    logger.LogCritical(result.Data);
                }
                logger.LogInformation(result.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRegisteredHandler] Error: {ex.Message}");

                logger.LogError(ex, "Error sending email or notification for user registration");

                await auditLogRepository.AddAsync(new AuditLog(
               userId: @event.UserId,
               action: "User Registration Failed",
               entityName: "User",
               entityId: @event.UserId,
               details: $"Registration failed: {ex.Message}",
               ip: ip,
               userAgent: userAgent
               ));
                
            }
        }
    }
}
