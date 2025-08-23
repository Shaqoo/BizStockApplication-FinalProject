using Domain.Exceptions;

namespace Domain.Entities
{
    public class MessageReaction 
    {
        public Guid Id { get; private init; }
        public Guid MessageId { get; private set; }
        public ChatMessage Message { get; private set; } = default!;
        public Guid ReactedByUserId { get; private set; }
        public User ReactedBy { get; private set; } = default!;
        public string Emoji { get; private set; } = default!; 
        public DateTimeOffset ReactedAt { get; private init; } = DateTimeOffset.Now;

        private MessageReaction() { }

        public MessageReaction(Guid messageId, Guid userId, string emoji)
        {
            if (string.IsNullOrWhiteSpace(emoji))
                throw new DomainException("Emoji is required.");

            MessageId = messageId;
            ReactedByUserId = userId;
            Emoji = emoji;
        }

        public void UpdateEmoji(string emoji)
        {
            if (string.IsNullOrWhiteSpace(emoji))
                throw new DomainException("Emoji cannot be empty.");
            Emoji = emoji;
        }

    }

}
