using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public class NotificationRequest
    {
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string Type { get; set; } = "info";  
        public Guid? UserId { get; set; }
        public Role Role { get; set; } = Role.None;
    }

}
