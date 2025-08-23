using Domain.Auditable;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditLog 
    {
        [Keyword(Name = "id")]
        public Guid Id { get; private init; } = Guid.NewGuid();
        [Keyword(Name = "userId")]
        public Guid UserId { get; private set; }
        [Date(Name = "timestamp")]
        public DateTime Timestamp { get; private init; } = DateTime.UtcNow;
        [Text(Name = "action")]
        public string Action { get; private set; } = default!;
        [Text(Name = "entityName")]
        public string? EntityName { get; private set; }
        [Text(Name = "entityName")]
        public Guid? EntityId { get; private set; }
        [Text(Name = "description")]
        public string? Description { get; private set; }
        [Text(Name = "ipAddress")]
        public string? IpAddress { get; private set; }
        [Text(Name = "userAgent")]
        public string? UserAgent { get; private set; }
        private AuditLog() { }

        public AuditLog(Guid userId, string action, string? entityName = null, Guid? entityId = null, string? details = null, string? ip = null, string? userAgent = null)
        {
            UserId = userId;
            Action = action;
            EntityName = entityName;
            EntityId = entityId;
            Description = details;
            IpAddress = ip;
            UserAgent = userAgent;
        }
    }

}
