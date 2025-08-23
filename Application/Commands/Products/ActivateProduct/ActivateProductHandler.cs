using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Products.ActivateProduct
{
    public class ActivateProductHandler(
    IAuthService authService,
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    IAuditLogRepository logRepository,
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    ILogger<ActivateProductHandler> logger)
    : IRequestHandler<ActivateProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not found.");

            if (user.RoleName is not ("Admin" or "Manager"))
                return Result<string>.Failure("You are not authorized to activate products.");

            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found.");

            if (!product.IsActive)
                return Result<string>.Failure("Product is already active.");

            await unitOfWork.BeginTransactionAsync();

            product.Activate();
            await productRepository.UpdateAsync(product);

            await logRepository.AddAsync(new AuditLog(
                user.Id,
                "ActivateProduct",
                nameof(Product),
                product.Id,
                "Product activated",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent));

            await unitOfWork.CommitTransactionAsync();

            await publishEndpoint.Publish(new ProductActivatedEvent
            {
                ProductId = product.Id,
                ActivatedByUserId = user.Id,
                ActivatedAt = DateTime.UtcNow,
                ProductName = product.Name,
                ActivatedByUserName = user.Email,
            });

            await mediator.Publish(new ProductActivatedEvent
            {
                ProductId = product.Id,
                ActivatedByUserId = user.Id,
                ActivatedAt = DateTime.UtcNow,
                ProductName = product.Name,
                ActivatedByUserName = user.Email,
            });

            logger.LogInformation("Product {ProductId} activated by {Email}", product.Id, user.Email);

            return Result<string>.Success("Product activated successfully.");
        }
    }

}
