using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record AuditLogReadDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public DateTime Timestamp { get; init; }
        public string Action { get; init; } = default!;
        public string? EntityName { get; init; }
        public Guid? EntityId { get; init; }
        public string? Description { get; init; }
        public string? IpAddress { get; init; }
        public string? UserAgent { get; init; }
    }

}
