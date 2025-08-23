using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Categories.Move
{
    public class MoveCategoryHandler(
    ICategoryRepository categoryRepository,
    IAuditLogRepository logRepository,
    IAuthService authService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<MoveCategoryCommand, Result<CategoryDto>>
    {
        public async Task<Result<CategoryDto>> Handle(MoveCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var category = await categoryRepository.GetByIdAsync(request.categoryId);
            if (category == null)
                return Result<CategoryDto>.Failure("Category not found.");

            Category? newParent = null;
            if (dto.NewParentCategoryId != null)
            {
                if (dto.NewParentCategoryId == request.categoryId)
                    return Result<CategoryDto>.Failure("Category cannot be its own parent.");

                newParent = await categoryRepository.GetByIdAsync(dto.NewParentCategoryId.Value);
                if (newParent == null)
                    return Result<CategoryDto>.Failure("New parent category not found.");
            }

            category.MoveToParent(dto.NewParentCategoryId, newParent?.Depth ?? 0);

            await unitOfWork.BeginTransactionAsync();
            await unitOfWork.CommitTransactionAsync();

            await logRepository.AddAsync(new AuditLog(
                userId: authService.CurrentUser()!.Id,
                action: $"Moved category '{category.Name}' to new parent",
                entityName: nameof(Category),
                entityId: category.Id
            ));

            return Result<CategoryDto>.Success(category.CategoryAsDto());
        }
    }

}
