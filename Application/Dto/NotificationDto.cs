namespace Application.Dto
{
    public record NotificationDto
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Type { get; set; } = "info"; 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Guid? ThreadId { get; set; } 
    }

}
