using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Products.GetById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;
    
}
