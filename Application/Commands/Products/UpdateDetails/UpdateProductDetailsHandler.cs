using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Products.UpdateDetails
{
    public class UpdateProductDetailsHandler(
    IAuthService authService,
    IProductRepository productRepository,
    IAuditLogRepository auditLogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator
) : IRequestHandler<UpdateProductDetailsCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("Unauthorized");

            var product = await productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found");

            var dto = request.ProductDetails;
            await unitOfWork.BeginTransactionAsync();

            if (!string.IsNullOrWhiteSpace(dto.Name))
                product.UpdateName(dto.Name);

            if (!string.IsNullOrWhiteSpace(dto.Description))
                product.UpdateDescription(dto.Description);

            product.UpdateUnitOfMeasure(dto.UnitOfMeasure);

            await productRepository.UpdateAsync(product);

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "UpdateProductDetails",
                nameof(Product),
                product.Id,
                $"Updated fields: {(dto.Name != null ? "Name, " : "")}{(dto.Description != null ? "Description, " : "")}{("UnitOfMeasure")}".Trim().TrimEnd(','),
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

            await unitOfWork.CommitTransactionAsync();

            await mediator.Publish(new ProductDetailsUpdatedEvent(product.Id,product.Name ,user.Id));

            return Result<string>.Success("Product details updated.");
        }
    }

}
