using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record CreateWarehouseDto
    {
        public required string Name { get; init; }
        public required string Location { get; init; }
    }

    public record UpdateWarehouseDto
    {
        public required string Name { get; init; }
        public required string Location { get; init; }
    }

}
