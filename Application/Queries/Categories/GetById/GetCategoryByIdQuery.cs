using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Categories.GetById
{
    public record GetCategoryByIdQuery(Guid id) : IRequest<Result<CategoryDto>>;
}
