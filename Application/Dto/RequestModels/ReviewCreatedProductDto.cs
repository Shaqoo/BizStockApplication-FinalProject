using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class ReviewCreatedProductDto
    {
        public required Guid ProductId { get; set; }
        public required bool Approved { get; set; }
    }

}
