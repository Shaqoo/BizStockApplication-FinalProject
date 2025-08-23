namespace Application.Dto
{
    using Domain.Enums;

    public record LostAccessRequestDto
    {
        public Guid Id { get; set; }
        public string UserIdentifier { get; set; } = default!;
        public string? AlternateEmail { get; set; }
        public string? AlternatePhone { get; set; }
        public string ProblemDescription { get; set; } = default!;
        public LostAccessStatus Status { get; set; }
        public string? AdminNotes { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }

}
