using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public class UserRegisteredEvent : INotification
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string FullName { get; init; } = string.Empty!;
        public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
        public string ManualEntryKey { get; init; } = string.Empty;
        public string QrCodeImageUrl { get; init; } = string.Empty;
        public UserRegisteredEvent(Guid userId, string email, string fullName, string manualEntryKey, string qrCodeImageUrl)
        {
            UserId = userId;
            Email = email;
            FullName = fullName;
            ManualEntryKey = manualEntryKey;
            QrCodeImageUrl = qrCodeImageUrl;
        }
    }

}
