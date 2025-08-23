using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatMessages.SendMessage
{
    public record SendMessageCommand(SendMessageRequest SendMessageRequest,RequestMetadata RequestMetadata) : IRequest<Result<MessageDto>>;

}
