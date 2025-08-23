using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record WarehouseDto(
    Guid Id,
    string Name,
    string Location,
    bool IsActive,
    int ItemCount
    );

}
