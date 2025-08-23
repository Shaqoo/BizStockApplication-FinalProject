using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record DeliveryAgentDto(Guid Id,string Fullname,string Email,string VehicleNo,string Contact,
        DeliveryAvailabilityStatus AvailabilityStatus);
    
}
