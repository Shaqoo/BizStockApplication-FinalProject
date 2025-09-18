namespace Application.Dto.RequestModels
{
    public record RequestMetadata(string UserAgent, string? IpAddress = null);

}
