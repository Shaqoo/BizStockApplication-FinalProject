using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record CustomerDto(Guid Id,string Fullname,Guid CustomerTypeId,string? TaxId,string? State,string? Address,
        string BusinessName);
}
