using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.RecentlyCheckedProduct.ClearRecentlyViewedProducts
{
    public class ClearRecentlyViewedProductsCommandHandler : IRequestHandler<ClearRecentlyViewedProductsCommand, Result<Unit>>
    {
        private readonly IRecentlyViewedProductRepository _repository;
        private readonly IMemoryCacheService _cacheService;
        private readonly IAuthService _authService;
        private readonly ILogger<ClearRecentlyViewedProductsCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ClearRecentlyViewedProductsCommandHandler(
            IRecentlyViewedProductRepository repository,
            IMemoryCacheService cacheService,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            ILogger<ClearRecentlyViewedProductsCommandHandler> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(ClearRecentlyViewedProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUser = _authService.CurrentUser();
                if ((currentUser == null || currentUser.Id == Guid.Empty) && string.IsNullOrWhiteSpace(request.SessionId))
                {
                    return Result<Unit>.Failure("Session ID is required for anonymous users.");
                }

                var cacheKey = currentUser == null || currentUser.Id == Guid.Empty
                    ? $"recently_viewed_session_{request.SessionId}"
                    : $"recently_viewed_user_{currentUser.Id}";

                var entity = currentUser == null || currentUser.Id == Guid.Empty
                    ? await _repository.GetBySessionIdAsync(request.SessionId!)
                    : await _repository.GetByUserIdAsync(currentUser.Id);

                if (entity != null)
                {
                    entity.ClearItems();
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                await _cacheService.RemoveAsync(cacheKey);

                _logger.LogInformation("Cleared recently viewed products for user {UserId}", currentUser?.Id.ToString() ?? request.SessionId);

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing recently viewed products for user {UserId}", request.SessionId ?? "User");
                return Result<Unit>.Failure("Failed to clear recently viewed products.");
            }
        }

    }

}
