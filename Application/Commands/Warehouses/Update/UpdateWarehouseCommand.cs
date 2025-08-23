using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Warehouses.Update
{
    public record UpdateWarehouseCommand(Guid Id,UpdateWarehouseDto Update)
    : IRequest<Result<Guid>>;

}
