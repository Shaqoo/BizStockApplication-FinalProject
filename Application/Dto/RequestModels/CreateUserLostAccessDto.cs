using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public class CreateLostAccessRequestDto
    {
        public string UserIdentifier { get; set; } = string.Empty;
        public string? AlternateEmail { get; set; }
        public string? AlternatePhone { get; set; }
        public string ProblemDescription { get; set; } = string.Empty;
    }

    public class UpdateLostAccessRequestDto
    {
        public LostAccessStatus Status { get; set; }
        public string? AdminNotes { get; set; }
    }

}
