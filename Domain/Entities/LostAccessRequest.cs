using Domain.Enums;

namespace Domain.Entities
{
    public class LostAccessRequest
    {
        public Guid Id { get; private set; }

        public string UserIdentifier { get; private set; } = default!;
        public string? AlternateEmail { get; private set; }
        public string? AlternatePhone { get; private set; }
        public string ProblemDescription { get; private set; } = default!;

        public LostAccessStatus Status { get; private set; }

        public string? AdminNotes { get; private set; }
        public DateTimeOffset SubmittedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        private LostAccessRequest() { }

        public LostAccessRequest(string userIdentifier, string problemDescription, string? alternateEmail = null, string? alternatePhone = null)
        {
            Id = Guid.NewGuid();
            UserIdentifier = userIdentifier;
            ProblemDescription = problemDescription;
            AlternateEmail = alternateEmail;
            AlternatePhone = alternatePhone;
            Status = LostAccessStatus.Pending;
            SubmittedAt = DateTime.UtcNow;
        }

       
        public void MarkInReview(string? adminNotes = null)
        {
            Status = LostAccessStatus.InReview;
            AdminNotes = adminNotes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Approve(string? adminNotes = null)
        {
            Status = LostAccessStatus.Resolved;
            AdminNotes = adminNotes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject(string? adminNotes = null)
        {
            Status = LostAccessStatus.Rejected;
            AdminNotes = adminNotes;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
