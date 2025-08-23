using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Tags.GetById
{
    public record GetTagByIdQuery(Guid Id) : IRequest<Result<TagDto>>;

}
