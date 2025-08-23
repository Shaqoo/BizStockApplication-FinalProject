using Domain.Auditable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid RecipientId { get; private set; }
        public User Recipient { get; private set; } = default!;
        public string Title { get; private set; } = default!;
        public string Message { get; private set; } = default!;
        public string Type { get; private set; } = "info"; 
        public bool IsRead { get; private set; } = false;
        public string? LinkUrl { get; private set; }

        private Notification() { }

        public Notification(Guid recipientId, string title, string message,string type = "info", string? linkUrl = null)
        {
            RecipientId = recipientId;
            Title = title;
            Type = type;
            Message = message;
            LinkUrl = linkUrl;
        }

        public void MarkAsRead()
        {
            IsRead = true;
            Modified();
        }
    }

}
