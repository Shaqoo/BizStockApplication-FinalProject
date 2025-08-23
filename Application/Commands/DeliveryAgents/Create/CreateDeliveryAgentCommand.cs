using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeliveryAgents.Create
{
    public record CreateDeliveryAgentCommand(CreateDeliveryAgentModel Model,RequestMetadata RequestMetadata) : IRequest<Result<TwoFactorSetupDto>>;

}