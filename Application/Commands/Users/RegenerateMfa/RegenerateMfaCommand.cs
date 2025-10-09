using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.RegenerateMfa
{
    public record RegenerateMfaCommand(string code,RequestMetadata RequestMetadata) : IRequest<Result<TwoFactorSetupDto>>;

}
