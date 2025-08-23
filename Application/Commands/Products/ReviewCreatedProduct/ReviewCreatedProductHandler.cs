using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Products.ReviewCreatedProduct
{
    public class ReviewCreatedProductHandler(
    IAuthService authService,
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    IAuditLogRepository logRepository,
    IMediator mediator,
    ILogger<ReviewCreatedProductHandler> logger)
    : IRequestHandler<ReviewCreatedProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ReviewCreatedProductCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
            {
                logger.LogWarning("Review failed: unauthenticated access.");
                return Result<string>.Failure("User not found.");
            }

            if (user.RoleName is not "Admin" and not "Manager")
            {
                logger.LogWarning("Unauthorized review attempt by {Email} with role {Role}.", user.Email, user.RoleName);
                return Result<string>.Failure("You are not authorized to review products.");
            }

            var product = await productRepository.GetByIdAsync(request.ReviewProductDto.ProductId);
            if (product == null)
            {
                logger.LogWarning("Product with ID {ProductId} not found for review by {Email}.", request.ReviewProductDto.ProductId, user.Email);
                return Result<string>.Failure("Product not found.");
            }

            await unitOfWork.BeginTransactionAsync();

            string action = request.ReviewProductDto.Approved ? "Approved Product" : "Rejected Product";

            if (request.ReviewProductDto.Approved)
            {
                product.Approve(user.Email);
                logger.LogInformation("Product {ProductId} approved by {Email}.", product.Id, user.Email);
            }
            else
            {
                product.Reject(user.Email);
                logger.LogInformation("Product {ProductId} rejected by {Email}.", product.Id, user.Email);
            }

            await productRepository.UpdateAsync(product);
            await unitOfWork.CommitTransactionAsync();

            var auditLog = new AuditLog(
                userId: user.Id,
                action: action,
                entityName: nameof(Product),
                entityId: product.Id,
                details: $"Product Name: {product.Name}, SKU: {product.SKU}, Approved: {request.ReviewProductDto.Approved}, Reviewer: {user.Email}",
                ip: request.RequestMetadata.IpAddress,
                userAgent: request.RequestMetadata.UserAgent
            );

            await logRepository.AddAsync(auditLog);

            logger.LogInformation("Audit log saved for product {ProductId} reviewed by {Email}.", product.Id, user.Email);

            await mediator.Publish(new ProductReviewCreatedEvent
            (
               product.Id,
               product.Name,
               product.CreatedBy
            ));

            return Result<string>.Success(request.ReviewProductDto.Approved
                ? "Product approved successfully."
                : "Product rejected successfully.");
        }
    }


}
