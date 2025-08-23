using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatMessages.GetMessageById
{
    public record GetMessageByIdQuery(Guid MessageId) : IRequest<Result<MessageDto>>;
}
