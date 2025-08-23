using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Warehouses.Create
{
    public record CreateWarehouseCommand(CreateWarehouseDto WarehouseDto,RequestMetadata RequestMetadata) : IRequest<Result<WarehouseDto>>;


}
