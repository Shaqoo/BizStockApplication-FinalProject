using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Tags.Create
{
    public record CreateTagCommand(CreateTagRequest Request,RequestMetadata RequestMetadata) : IRequest<Result<Guid>>;
}
