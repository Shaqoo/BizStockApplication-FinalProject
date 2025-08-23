using Domain.Auditable;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; private init; }
        public Guid ChatThreadId { get; private set; }
        public ChatThread ChatThread { get; private set; } = default!;
        public Guid SenderId { get; private set; } 
        public User Sender { get; private set; } = default!;
        public string? Message { get; private set; }
        public string? AudioUrl { get; private set; }
        public string? PictureUrl { get; private set; }
        public Guid? RepliedToMessageId { get; private set; }
        public ChatMessage? RepliedToMessage { get; private set; }
        public bool IsRead { get; private set; } = false;
        public DateTimeOffset SentAt { get; private init; } = DateTimeOffset.Now;
        public ICollection<MessageReaction> Reactions { get; private set; } = new List<MessageReaction>();

        private ChatMessage() { }

        public ChatMessage(Guid chatThreadId, Guid senderId, string? message = null, string? audioUrl = null, string? pictureUrl = null, Guid? replyToId = null)
        {
            if (string.IsNullOrWhiteSpace(message) &&
                string.IsNullOrWhiteSpace(audioUrl) &&
                string.IsNullOrWhiteSpace(pictureUrl))
            {
                throw new DomainException("A message must contain text, an audio file, or a picture.");
            }

            ChatThreadId = chatThreadId;
            SenderId = senderId;
            Message = message;
            AudioUrl = audioUrl;
            PictureUrl = pictureUrl;
            RepliedToMessageId = replyToId;
        }



        public void MarkAsRead()
        {
            IsRead = true;
        }
    }

}
