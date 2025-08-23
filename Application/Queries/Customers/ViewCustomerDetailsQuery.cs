using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Customers
{
    public record ViewCustomerDetailsQuery : IRequest<Result<CustomerDto>>;
     
}
