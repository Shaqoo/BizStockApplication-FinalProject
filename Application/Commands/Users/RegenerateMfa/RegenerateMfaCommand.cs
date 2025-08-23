using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.RegenerateMfa
{
    public record RegenerateMfaCommand(RequestMetadata RequestMetadata) : IRequest<Result<TwoFactorSetupDto>>;

}
