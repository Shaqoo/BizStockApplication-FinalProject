using Application.Dto;
using Application.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.StockMovements.GetAll
{
    public record GetAllStockMovementsQuery(PageRequest PageRequest) : IRequest<Result<PaginatedList<StockMovementDto>>>;
    
}
