using Microsoft.AspNetCore.Http;

namespace Application.Dto.RequestModels
{
    public record SendMessageRequest(
    Guid ChatThreadId,
    string? Message,
    IFormFile? Audio,
    IFormFile? Picture,
    Guid? RepliedToMessageId);


}
