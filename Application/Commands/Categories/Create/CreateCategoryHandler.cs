using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Categories.Create
{
    public class CreateCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IAuditLogRepository logRepository,
    IAuthService authService
) : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.Dto == null)
                return Result<CategoryDto>.Failure("Category data cannot be null.");

            if (request.Dto.ParentCategoryId is not null)
            {
                var parentCategory = await categoryRepository.GetByIdAsync(request.Dto.ParentCategoryId.Value);
                if (parentCategory == null)
                    return Result<CategoryDto>.Failure("Parent category not found.");
            }

            var existsInSameParent = await categoryRepository.GetByExpression(
                c => c.Name == request.Dto.Name && c.ParentCategoryId == request.Dto.ParentCategoryId
            );
            if (existsInSameParent != null)
                return Result<CategoryDto>.Failure("Category name must be unique within the same parent category.");


            var category = new Category(
                request.Dto.Name,
                request.Dto.Description,
                request.Dto.ParentCategoryId
            );

            try
            {
                await unitOfWork.BeginTransactionAsync();

                await categoryRepository.AddAsync(category);

                await logRepository.AddAsync(new AuditLog(
                    userId:authService.CurrentUser()!.Id,
                    action: "Create Category",
                    entityName: nameof(Category),
                    entityId: category.Id,
                    details: $"Created category '{category.Name}'"
                ));

                await unitOfWork.CommitTransactionAsync();

                var dto = new CategoryDto(
                    category.Id,
                    category.Name,
                    category.Description,
                    category.Depth,
                    category.ParentCategoryId,
                    0
                );

                return Result<CategoryDto>.Success(dto);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<CategoryDto>.Failure("An error occurred while creating the category: " + ex.Message);
            }
        }
    }

}
