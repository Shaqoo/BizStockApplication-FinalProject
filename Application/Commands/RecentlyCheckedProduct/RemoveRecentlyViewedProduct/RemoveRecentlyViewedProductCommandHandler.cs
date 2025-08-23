using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.RecentlyCheckedProduct.RemoveRecentlyViewedProduct
{
    public class RemoveRecentlyViewedProductCommandHandler : IRequestHandler<RemoveRecentlyViewedProductCommand, Result<Unit>>
    {
        private readonly IRecentlyViewedProductRepository _repository;
        private readonly IMemoryCacheService _cacheService;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveRecentlyViewedProductCommandHandler> _logger;

        public RemoveRecentlyViewedProductCommandHandler(
            IRecentlyViewedProductRepository repository,
            IMemoryCacheService cacheService,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            ILogger<RemoveRecentlyViewedProductCommandHandler> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(RemoveRecentlyViewedProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = _authService.CurrentUser();

                if ((currentUser == null || currentUser.Id == Guid.Empty) && string.IsNullOrWhiteSpace(request.sessionId))
                {
                    return Result<Unit>.Failure("User not authenticated and session ID is missing.");
                }

                var cacheKey = (currentUser == null || currentUser.Id == Guid.Empty)
                    ? $"recently_viewed_session_{request.sessionId}"
                    : $"recently_viewed_user_{currentUser.Id}";

                var entity = (currentUser == null || currentUser.Id == Guid.Empty)
                    ? await _repository.GetBySessionIdAsync(request.sessionId!)
                    : await _repository.GetByUserIdAsync(currentUser.Id);

                if (entity == null)
                {
                    _logger.LogWarning("No recently viewed product list found for user/session.");
                    return Result<Unit>.Failure("No recently viewed products found.");
                }

                var removed = entity.RemoveItem(request.ProductId);

                if (!removed)
                {
                    _logger.LogWarning("Product {ProductId} not found in recently viewed list.", request.ProductId);
                    return Result<Unit>.Failure("Product not found in recently viewed list.");
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _cacheService.RemoveAsync(cacheKey);

                _logger.LogInformation("Removed product {ProductId} from recently viewed for {IdType} {Id}",
                    request.ProductId,
                    currentUser?.Id != Guid.Empty ? "user" : "session",
                    currentUser?.Id.ToString() ?? request.sessionId
                );

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing product {ProductId} from recently viewed.", request.ProductId);
                return Result<Unit>.Failure("Failed to remove product from recently viewed.");
            }
        }
    }
}
