using Application.Dto;
using Application.Pagination;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Products.GetProductsByStatus
{
    public record GetProductsByStatusQuery(PageRequest PageRequest,ProductStatus ProductStatus) : IRequest<Result<PaginatedList<ProductDto>>>;

}
