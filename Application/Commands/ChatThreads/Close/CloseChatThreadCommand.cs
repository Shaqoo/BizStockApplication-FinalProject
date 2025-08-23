using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatThreads.Close
{
    public record CloseChatThreadCommand(Guid ChatThreadId, RequestMetadata RequestMetadata) : IRequest<Result<string>>;

}
