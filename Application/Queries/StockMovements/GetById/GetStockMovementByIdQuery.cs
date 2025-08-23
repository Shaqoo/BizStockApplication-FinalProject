using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.StockMovements.GetById
{
    public record GetStockMovementByIdQuery(Guid Id): IRequest<Result<StockMovementDto>>;
    
}
