using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Categories.Delete
{
    public class DeleteCategoryHandler(
    ICategoryRepository categoryRepository,
    IAuditLogRepository logRepository,
    IAuthService authService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not authenticated.");

            var category = await categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null || category.IsDeleted)
                return Result<string>.Failure("Category not found or already deleted.");
            
            var hasProduct = await categoryRepository.HasProductsAsync(request.CategoryId);
            if (hasProduct)
                return Result<string>.Failure("Category cannot be deleted because it has associated products.");

            if(category.SubCategories.Any())
                return Result<string>.Failure("Category cannot be deleted because it has subcategories.");

            category.ToogleDelete();

            await unitOfWork.BeginTransactionAsync();
            await unitOfWork.CommitTransactionAsync();

            await logRepository.AddAsync(new AuditLog(
                userId: user.Id,
                action: $"Soft deleted category '{category.Name}'",
                entityName: nameof(Category),
                entityId: category.Id
            ));

            return Result<string>.Success("Category Deleted Successfully");
        }
    }

}
