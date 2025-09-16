using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Products.Create
{
    public class CreateProductHandler(IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IBrandRepository brandRepository,
        ICategoryRepository categoryRepository,
        IUploadService uploadService,
        IAuthService authService,
        IPublishEndpoint publishEndpoint,
        IMediator mediator,
        IAuditLogRepository logRepository,
        ILogger<CreateProductHandler> logger) : IRequestHandler<CreateProductCommand, Result<ProductDto>>
    {
        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
             if(request.RequestModel is null)
                return Result<ProductDto>.Failure("Request model cannot be null.");

            var product = await productRepository.GetByExpression(a => a.SKU == request.RequestModel.SKU ||
            a.Barcode == request.RequestModel.Barcode);

            if (product is not null)
            {
                return Result<ProductDto>.Failure("Product with the same SKU or Barcode already exists.");
            }

            var brand = await brandRepository.GetByIdAsync(request.RequestModel.BrandId);
            if (brand is null)
            {
                return Result<ProductDto>.Failure("Brand not found.");
            }

            var category = await categoryRepository.GetByIdAsync(request.RequestModel.CategoryId);
            if (category is null)
            {
                return Result<ProductDto>.Failure("Category not found.");
            }

            if (request.RequestModel.ImageUrl is null)
            {
                return Result<ProductDto>.Failure("Product Image cannot be null");
            }

           

            using var imageStream = request.RequestModel.ImageUrl.OpenReadStream();
            var imageUrl = await uploadService.UploadProductImageAsync(imageStream, request.RequestModel.ImageUrl.FileName);

            var qrDataObject = new
            {
                request.RequestModel.Name,
                request.RequestModel.SellingPrice,
                request.RequestModel.CostPrice,
                imageUrl
            };
            string qrPayload = System.Text.Json.JsonSerializer.Serialize(qrDataObject);

            var qrCodePath = await uploadService.UploadQrCodeAsync(qrPayload);
            if (!qrCodePath.IsSuccess)
            {
                qrCodePath = await uploadService.UploadQrCodeAsync(request.RequestModel.QrCodeValue);
            }

            var newProduct = new Product(
                name: request.RequestModel.Name,
                sku: request.RequestModel.SKU,
                barcode: request.RequestModel.Barcode,
                qrCodeValue:  qrCodePath.Data ?? "",
                imageUrl: imageUrl,
                costPrice: request.RequestModel.CostPrice,
                sellingPrice: request.RequestModel.SellingPrice,
                unitOfMeasure: request.RequestModel.UnitOfMeasure,
                description: request.RequestModel.Description,
                categoryId: request.RequestModel.CategoryId,
                brandId: request.RequestModel.BrandId
            );

            newProduct.SetCreatedBy(authService.CurrentUser()!.Email);

            await unitOfWork.BeginTransactionAsync();
            try
            {
                await productRepository.AddAsync(newProduct);
                await unitOfWork.CommitTransactionAsync();

                await mediator.Publish(new ProductCreatedEvent(newProduct.Id,newProduct.Name,newProduct.SKU,
                    newProduct.BrandId,authService.CurrentUser()!.Id,DateTime.UtcNow),cancellationToken);

                await publishEndpoint.Publish(new ProductCreatedEvent(newProduct.Id, newProduct.Name, newProduct.SKU,
                    newProduct.BrandId, authService.CurrentUser()!.Id, DateTime.UtcNow), cancellationToken);

                logger.LogInformation("Product created successfully: {ProductId}", newProduct.Id);

                await logRepository.AddAsync(new AuditLog(
                    userId: authService.CurrentUser()!.Id,
                    action: "CreateProduct",
                    entityName: nameof(Product),
                    entityId: newProduct.Id,
                    details: $"Created product with SKU: {newProduct.SKU}, Name: {newProduct.Name}",
                    ip: request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                    ));

                var productDto = newProduct.ToDto();
                return Result<ProductDto>.Success(productDto);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();

                await logRepository.AddAsync(new AuditLog(
                    userId: authService.CurrentUser()!.Id,
                    action: "CreateProductError",
                    entityName: nameof(Product),
                    entityId:Guid.Empty,
                    ip:request.RequestMetadata.IpAddress,
                    userAgent:request.RequestMetadata.UserAgent));
                logger.LogError(ex, "Error creating product");

                return Result<ProductDto>.Failure("An error occurred while creating the product.");
            }

        }
    }
}
