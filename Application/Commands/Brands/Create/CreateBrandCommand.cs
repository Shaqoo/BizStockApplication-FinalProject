using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Brands.Create
{
    public record CreateBrandCommand(CreateBrandDto Dto,RequestMetadata RequestMetadata) : IRequest<Result<Guid>>;

}
