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
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public AddRecentlyViewedProductCommandHandler(
            IRecentlyViewedProductRepository repository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddRecentlyViewedProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Handle(AddRecentlyViewedProductCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            RecentlyViewedProducts? entity = null;

            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if(product == null)
                    return Result<string>.Failure("Product not found.");

                await _unitOfWork.BeginTransactionAsync();
                if(request.UserId.HasValue)
{
                    var user = await _userRepository.GetByIdAsync(request.UserId.Value);
                    if (user == null)
                        return Result<string>.Failure("User not found.");

                    entity = await _repository.GetByUserIdAsync(request.UserId.Value);
                    if (entity == null)
                    {
                        entity = new RecentlyViewedProducts(request.UserId.Value);
                        await _repository.AddAsync(entity);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
                else if (!string.IsNullOrEmpty(request.SessionId))
                {
                    entity = await _repository.GetBySessionIdAsync(request.SessionId);
                    if (entity == null)
                    {
                        entity = new RecentlyViewedProducts(request.SessionId);
                        await _repository.AddAsync(entity);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    return Result<string>.Failure("Either UserId or SessionId is required.");
                }
                var existing = entity.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
                if (existing != null)
                   await _repository.DeleteItemAsync(existing);
                entity.RemoveLastItem();

                await _repository.AddProductAsync(new RecentlyViewedProduct(entity.Id,request.ProductId));
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
