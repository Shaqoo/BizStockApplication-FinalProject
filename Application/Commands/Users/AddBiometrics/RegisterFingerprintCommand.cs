using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.AddBiometrics
{
    public record RegisterFingerprintCommand(FingerprintRegistrationDto RegistrationDto,RequestMetadata RequestMetadata) : IRequest<Result<object>>;
     
}
