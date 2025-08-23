using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Notifications.GetNotificationsByRecipient
{
    public class GetNotificationsByRecipientPagedHandler
        : IRequestHandler<GetNotificationsByRecipientPagedQuery, Result<PaginatedList<NotificationDto>>>
    {
        private readonly INotificationRepository _repository;
        private readonly IMemoryCacheService _cache;
        private readonly ILogger<GetNotificationsByRecipientPagedHandler> _logger;

        public GetNotificationsByRecipientPagedHandler(
            INotificationRepository repository,
            IMemoryCacheService cache,
            ILogger<GetNotificationsByRecipientPagedHandler> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<NotificationDto>>> Handle(
            GetNotificationsByRecipientPagedQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"notifications:paged:{request.RecipientId}:{request.PageRequest.Page}:{request.PageRequest.PageSize}";

                var pagedList = await _cache.GetOrAddAsync(cacheKey, async () =>
                {
                    return await _repository.GetByRecipientPagedAsync(request.RecipientId, request.PageRequest);
                }, TimeSpan.FromMinutes(5));

                return Result<PaginatedList<NotificationDto>>.Success(new PaginatedList<NotificationDto>(pagedList.Items
                    .Select(n => new NotificationDto
                    {
                        Title = n.Title,
                        Message = n.Message,
                        Type = n.Type,
                        Timestamp = n.DateCreated.DateTime
                    }).ToList(),pagedList.TotalCount,pagedList.PageNumber,pagedList.PageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paged notifications for recipient {RecipientId}", request.RecipientId);
                return Result<PaginatedList<NotificationDto>>.Failure("Failed to retrieve notifications.");
            }
        }
    }

}
