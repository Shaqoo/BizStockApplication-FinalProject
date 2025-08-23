using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatMessages.MarkAsRead
{
    public record MarkAsReadCommand(Guid MessageId) : IRequest<Result<string>>;

}

