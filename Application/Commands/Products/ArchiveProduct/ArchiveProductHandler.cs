using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.ArchiveProduct
{
    public class ArchiveProductHandler(
    IAuthService authService,
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    IAuditLogRepository logRepository,
    IMediator mediator,
    ILogger<ArchiveProductHandler> logger)
    : IRequestHandler<ArchiveProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ArchiveProductCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not found.");

            if (user.RoleName is not ("Admin" or "Manager"))
                return Result<string>.Failure("You are not authorized to archive products.");

            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found.");

            if (!product.IsActive)
                return Result<string>.Failure("Product is already archived.");

            await unitOfWork.BeginTransactionAsync();

            product.Archive();  
            await productRepository.UpdateAsync(product);

            await logRepository.AddAsync(new AuditLog(
                user.Id,
                "ArchiveProduct",
                nameof(Product),
                product.Id,
                "Product archived",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent));

            await unitOfWork.CommitTransactionAsync();

            logger.LogInformation("Product {ProductId} archived by {Email}", product.Id, user.Email);

            await mediator.Publish(new ProductArchivedEvent(product.Id, product.Name,user.Id), cancellationToken);

            return Result<string>.Success("Product archived successfully.");
        }
    }

}
