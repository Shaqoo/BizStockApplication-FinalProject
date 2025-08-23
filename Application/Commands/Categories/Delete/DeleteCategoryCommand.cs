using Application.Dto;
using MediatR;

namespace Application.Commands.Categories.Delete
{
    public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Result<string>>;

}
