namespace Application.Dto.RequestModels
{
    public class CreateCartRequest
    {
        public Guid? UserId { get; set; }
        public string SessionId { get; set; } = string.Empty;
    }
}
