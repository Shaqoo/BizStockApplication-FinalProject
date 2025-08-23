using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record UpdateProductDetailsDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public UnitOfMeasure UnitOfMeasure { get; set; }
    }

}
