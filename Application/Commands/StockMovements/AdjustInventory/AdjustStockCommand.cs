using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.StockMovements.AdjustInventory
{
    public record AdjustStockCommand(AdjustStockRequest Request,RequestMetadata RequestMetadata)
        : IRequest<Result<string>>;
    
}
