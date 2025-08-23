using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public record StockTransferredEvent(Guid ProductId, Guid FromWarehouseId, Guid ToWarehouseId, int Quantity,string ProductName,string FromWarehouseName,
        string ToWarehouseName,string? Reason)
        :INotification;

}
