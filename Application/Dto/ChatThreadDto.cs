using Domain.Enums;

namespace Application.Dto
{
    public record ChatThreadDto(Guid Id,ChatStatus Status,string CreatedBy,Guid? AgentId,DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

}
