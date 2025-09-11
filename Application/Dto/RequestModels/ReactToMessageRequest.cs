namespace Application.Dto.RequestModels
{
    public record ReactToMessageRequest(
        Guid MessageId,
        string Emoji);


}
