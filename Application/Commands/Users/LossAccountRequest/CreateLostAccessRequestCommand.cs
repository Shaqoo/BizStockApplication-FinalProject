using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Users.LossAccountRequest
{
    public record CreateLostAccessRequestCommand(CreateLostAccessRequestDto Dto,RequestMetadata RequestMetadata)
        : IRequest<Result<Guid>>;

}
