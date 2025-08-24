using Application.Dto;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Products.GetProductsByWarehouseId
{
    public record GetProductsByWarehouseIdQuery(Guid WarehouseId, PageRequest PageRequest) : IRequest<Result<PaginatedList<WarehouseProductDto>>>;

}
