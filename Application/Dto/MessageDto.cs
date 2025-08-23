namespace Application.Dto
{
    public record MessageDto(
    Guid Id,
    Guid ChatThreadId,
    Guid SenderId,
    string SenderName,
    string? Message,
    string? AudioUrl,
    string? PictureUrl,
    Guid? RepliedToMessageId,
    string? RepliedToMessagePreview,
    bool IsRead,
    DateTimeOffset SentAt,
    List<ReactionDto> Reactions
);

}
