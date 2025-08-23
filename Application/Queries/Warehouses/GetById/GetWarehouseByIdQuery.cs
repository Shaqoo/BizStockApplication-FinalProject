using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Warehouses.GetById
{
    public record GetWarehouseByIdQuery(Guid Id) : IRequest<Result<WarehouseDto>>;

}
