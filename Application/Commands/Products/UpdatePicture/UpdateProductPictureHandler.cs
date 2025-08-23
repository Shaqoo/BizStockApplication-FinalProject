using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Products.UpdatePicture
{
    public class UpdateProductPictureHandler(
    IAuthService authService,
    IProductRepository productRepository,
    IUploadService fileStorageService,
    IAuditLogRepository auditLogRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator
) : IRequestHandler<UpdateProductPictureCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateProductPictureCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("Unauthorized");

            var product = await productRepository.GetByIdAsync(request.PictureDto.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found");

            var stream = request.PictureDto.Picture.OpenReadStream();
            var pictureUrl = await fileStorageService.UploadProductImageAsync(stream,request.PictureDto.Picture.FileName);

            product.SetPicture(pictureUrl);  

            await unitOfWork.BeginTransactionAsync();
            await productRepository.UpdateAsync(product);

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "AddProductPicture",
                nameof(Product),
                product.Id,
                "Picture updated",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

            await unitOfWork.CommitTransactionAsync();

            await mediator.Publish(new ProductPictureUpdatedEvent(product.Id,product.Name ,user.Id), cancellationToken);

            return Result<string>.Success("Product picture uploaded successfully.");
        }
    }

}
