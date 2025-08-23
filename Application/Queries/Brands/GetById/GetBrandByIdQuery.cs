using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Brands.NewFolder
{
    public record GetBrandByIdQuery(Guid Id) : IRequest<Result<BrandDto>>;

}
