using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.DeActivateUser
{
    public record DeactivateUserCommand(RequestMetadata RequestMetadata,Guid Id) : IRequest<Result<string>>;
     
}
