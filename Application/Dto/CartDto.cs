namespace Application.Dto
{
    public record CartDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
        public bool IsLinked { get; set; }
    }
}
