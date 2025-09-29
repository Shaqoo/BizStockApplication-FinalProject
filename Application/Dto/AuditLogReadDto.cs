namespace Application.Dto
{
    public record AuditLogReadDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Fullname { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty ;
        public string? ProfilePic { get; init; }
        public DateTime Timestamp { get; init; }
        public string Action { get; init; } = default!;
        public string? EntityName { get; init; }
        public Guid? EntityId { get; init; }
        public string? Description { get; init; }
        public string? IpAddress { get; init; }
        public string? UserAgent { get; init; }
    }

}
