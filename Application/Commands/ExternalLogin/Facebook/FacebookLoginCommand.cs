using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ExternalLogin.Facebook
{
    public record FacebookLoginCommand(ExternalLoginDto Dto, RequestMetadata RequestMetadata) : IRequest<Result<AuthDto>>;
}
