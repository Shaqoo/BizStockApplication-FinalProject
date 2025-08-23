using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
   
    public class UserRecoveryCode
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Code { get; private set; } = default!;
        public bool IsUsed { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UsedAt { get; private set; }

        private UserRecoveryCode() { }  

        public UserRecoveryCode(Guid userId, string code)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Code = code ?? throw new DomainException(nameof(code));
            CreatedAt = DateTime.UtcNow;
            IsUsed = false;
        }

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new DomainException("Recovery code already used.");

            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }
    

}
