using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class RemoveCartItemRequest
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }
}
