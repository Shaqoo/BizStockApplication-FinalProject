using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Categories.Update
{
    public class UpdateCategoryHandler(
    ICategoryRepository categoryRepository,
    IAuditLogRepository logRepository,
    IAuthService authService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
    {
        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var category = await categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<CategoryDto>.Failure("Category not found.");


            var existsInSameParent = await categoryRepository.GetByExpression(
                c => c.Name == request.Dto.Name && c.ParentCategoryId == category.ParentCategoryId
            );
            if (existsInSameParent != null)
                return Result<CategoryDto>.Failure("Category name must be unique within the same parent category.");


            category.Update(dto.Name, dto.Description);

            await unitOfWork.BeginTransactionAsync();
            await unitOfWork.CommitTransactionAsync();

            await logRepository.AddAsync(new AuditLog(
                userId: authService.CurrentUser().Id,
                action: $"Updated category '{category.Name}'",
                entityName: nameof(Category),
                entityId: category.Id
            ));

            return Result<CategoryDto>.Success(category.CategoryAsDto());
        }
    }

}
