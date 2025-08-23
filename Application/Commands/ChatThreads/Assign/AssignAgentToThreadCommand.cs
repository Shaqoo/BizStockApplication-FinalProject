using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatThreads.Assign
{
    public record AssignAgentToThreadCommand(Guid ChatThreadId,Guid AgentId,RequestMetadata RequestMetadata) : IRequest<Result<string>>;

}
