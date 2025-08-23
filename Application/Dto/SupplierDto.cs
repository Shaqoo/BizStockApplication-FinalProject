using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record SupplierDto(Guid Id,string CompanyName,string Address,string PhoneNumber,string TaxId,string ContactPerson,string email);
   
}
