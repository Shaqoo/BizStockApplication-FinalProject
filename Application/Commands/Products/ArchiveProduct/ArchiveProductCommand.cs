using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.ArchiveProduct
{
    public record ArchiveProductCommand(Guid ProductId, RequestMetadata RequestMetadata)
    : IRequest<Result<string>>;

}
