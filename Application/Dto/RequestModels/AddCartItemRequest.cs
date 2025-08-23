using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class AddCartItemRequest
    {
        public Guid? UserId { get; set; }
        public string CartSessionId { get;private set; } = string.Empty;
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public void SetCartSessionId(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(CartSessionId))
            {
                CartSessionId = sessionId;
            }
        }
    }
}
