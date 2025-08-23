using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Categories.Update
{
    public record UpdateCategoryCommand(UpdateCategoryDto Dto, Guid CategoryId) : IRequest<Result<CategoryDto>>;

}
