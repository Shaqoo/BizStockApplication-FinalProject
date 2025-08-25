namespace Application.Dto.RequestModels
{
    public class SendAiMessageRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

}
