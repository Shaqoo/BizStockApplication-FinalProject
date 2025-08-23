using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fido2NetLib.AuthenticatorAttestationRawResponse;

namespace Application.Commands.Brands.Delete
{
    public record DeleteBrandCommand(
    Guid Id,
    RequestMetadata RequestMetadata
) : IRequest<Result<string>>;

}
