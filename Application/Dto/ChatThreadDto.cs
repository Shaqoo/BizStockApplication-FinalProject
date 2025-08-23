using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record ChatThreadDto(Guid Id,ChatStatus Status,Guid CreatedBy,Guid? AgentId,DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);

}
