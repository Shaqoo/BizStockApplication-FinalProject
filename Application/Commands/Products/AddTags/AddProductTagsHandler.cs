using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.AddTags
{
    public class AddProductTagsHandler(IUnitOfWork unitOfWork,
        ITagRepository tagRepository,
        IProductRepository productRepository,
        IAuthService authService,
        IAuditLogRepository auditLogRepository,
        ILogger<AddProductTagsHandler> logger) : IRequestHandler<AddProductTagsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(AddProductTagsCommand request, CancellationToken cancellationToken)
        {
            var tag = await tagRepository.GetByIdAsync(request.addProductTag.TagId);
            if (tag == null)
            {
                logger.LogError("Tag with ID {TagId} not found.", request.addProductTag.TagId);
                return Result<string>.Failure("Tag not found.");
            }
            var product = await productRepository.GetByIdAsync(request.addProductTag.ProductId);
            if (product == null)
            {
                logger.LogError("Product with ID {ProductId} not found.", request.addProductTag.ProductId);
                return Result<string>.Failure("Product not found.");
            }
            if (product.ProductTags.Any(t => t.TagId == tag.Id))
            {
                logger.LogWarning("Tag with ID {TagId} is already associated with product {ProductId}.", tag.Id, product.Id);
                return Result<string>.Failure("Tag is already associated with this product.");
            }
            var productTag = new ProductTag(product.Id,tag.Id);
            await unitOfWork.BeginTransactionAsync();
            product.AddTag(productTag);
            try
            {
                await productRepository.UpdateAsync(product);
                await unitOfWork.CommitTransactionAsync();
                await auditLogRepository.AddAsync(new AuditLog(
                    authService.CurrentUser()!.Id, "AddTagToProduct", nameof(Product), product.Id,
                    $"Tag {tag.Name} added to product {product.Name}.",request.requestMetadata.IpAddress,
                    request.requestMetadata.UserAgent));

                logger.LogInformation("Tag with ID {TagId} added to product with ID {ProductId}.", tag.Id, product.Id);

                return Result<string>.Success("Tag added to product successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding tag to product.");
                await unitOfWork.RollbackTransactionAsync();
                return Result<string>.Failure("An error occurred while adding the tag to the product.");
            }
        }
    }
}
