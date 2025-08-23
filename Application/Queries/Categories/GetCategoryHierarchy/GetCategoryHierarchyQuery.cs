using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Categories.GetCategoryHierarchy
{
    public record GetCategoryTreeQuery : IRequest<Result<List<CategoryTreeDto>>>;


}
