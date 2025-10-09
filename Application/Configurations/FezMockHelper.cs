using Application.Dto.RequestModels;

namespace Application.Configurations
{
    public static class FezHelper
    {
        public static void UpdateOrderHistory(TrackOrderResponseDto order)
        {
            if (order == null || order.CreatedAt == default)
                return;

            var now = DateTime.UtcNow;
            var daysSinceCreated = (now - order.CreatedAt).TotalDays;

            var history = new List<OrderHistoryDto>();

            
            history.Add(new OrderHistoryDto
            {
                OrderStatus = "Pending Pickup",
                StatusCreationDate = order.CreatedAt,
                StatusDescription = "Order created and awaiting pickup."
            });
            order.Status = "Pending Pickup";

            
            if (daysSinceCreated >= 0.5)
            {
                history.Add(new OrderHistoryDto
                {
                    OrderStatus = "Picked Up",
                    StatusCreationDate = order.CreatedAt.AddHours(6),
                    StatusDescription = "Package picked up by FEZ dispatch."
                });
                order.Status = "Picked Up";
            }

            
            if (daysSinceCreated >= 1)
            {
                history.Add(new OrderHistoryDto
                {
                    OrderStatus = "Dispatched",
                    StatusCreationDate = order.CreatedAt.AddHours(12),
                    StatusDescription = "Your package has been dispatched and is en route to the destination hub."
                });
                order.Status = "Dispatched";
            }

             
            if (daysSinceCreated >= 2)
            {
                history.Add(new OrderHistoryDto
                {
                    OrderStatus = "In Transit",
                    StatusCreationDate = order.CreatedAt.AddDays(1).AddHours(10),
                    StatusDescription = "Package is in transit and expected to arrive soon."
                });
                order.Status = "In Transit";
            }

             
            if (daysSinceCreated >= 3)
            {
                history.Add(new OrderHistoryDto
                {
                    OrderStatus = "Out for Delivery",
                    StatusCreationDate = order.CreatedAt.AddDays(2).AddHours(8),
                    StatusDescription = "Your package is out for delivery."
                });
                order.Status = "Out for Delivery";
            }

            
            if (daysSinceCreated >= 4)
            {
                history.Add(new OrderHistoryDto
                {
                    OrderStatus = "Delivered",
                    StatusCreationDate = order.CreatedAt.AddDays(3).AddHours(4),
                    StatusDescription = "Package successfully delivered to the customer."
                });
                order.Status = "Delivered";
            }

           
            if (daysSinceCreated < 0.5)
                order.Status = "pending pickup";
            else if (daysSinceCreated < 1)
                order.Status = "picked up";
            else if (daysSinceCreated < 2)
                order.Status = "dispatched";
            else if (daysSinceCreated < 3)
                order.Status = "in transit";
            else if (daysSinceCreated < 4)
                order.Status = "out for delivery";
            else
                order.Status = "delivered";

            
            order.History = history.OrderBy(x => x.StatusCreationDate).ToList();
        }
    }
}
