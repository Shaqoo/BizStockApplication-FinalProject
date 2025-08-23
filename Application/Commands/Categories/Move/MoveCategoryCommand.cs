using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Categories.Move
{
    public record MoveCategoryCommand(MoveCategoryDto Dto, Guid categoryId) : IRequest<Result<CategoryDto>>;

}
