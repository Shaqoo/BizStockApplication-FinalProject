using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.RecentlyCheckedProduct.AddRecentlyViewedProduct
{
    public class AddRecentlyViewedProductCommandHandler
        : IRequestHandler<AddRecentlyViewedProductCommand, Result<string>>
    {
        private readonly IRecentlyViewedProductRepository _repository;
        private readonly ILogger<AddRecentlyViewedProductCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AddRecentlyViewedProductCommandHandler(
            IRecentlyViewedProductRepository repository,
            IUnitOfWork unitOfWork,
            ILogger<AddRecentlyViewedProductCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(AddRecentlyViewedProductCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            RecentlyViewedProducts? entity = null;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (request.UserId.HasValue)
                {
                    entity = await _repository.GetByUserIdAsync(request.UserId.Value);
                    if (entity == null)
                    {
                        entity = new RecentlyViewedProducts(request.UserId.Value);
                        await _repository.AddAsync(entity);
                    }
                }
                else if (!string.IsNullOrEmpty(request.SessionId))
                {
                    entity = await _repository.GetBySessionIdAsync(request.SessionId);
                    if (entity == null)
                    {
                        entity = new RecentlyViewedProducts(request.SessionId);
                        await _repository.AddAsync(entity);
                    }
                }

                entity!.AddProduct(request.ProductId);
                await _repository.UpdateAsync(entity);
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Product {ProductId} added to RecentlyViewed list {ListId}",
                    request.ProductId, entity.Id);

                return Result<string>.Success("Product added to recently viewed list.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error while adding product {ProductId} to RecentlyViewed", request.ProductId);
                return Result<string>.Failure("Failed to add product.");
            }
        }
    }

}
