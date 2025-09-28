using Domain.Exceptions;

namespace Domain.Auditable
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
        public DateTimeOffset DateCreated { get; private init; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastModified { get; private set; } = DateTimeOffset.UtcNow;
        public string CreatedBy { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public void ToogleDelete()
        {
            IsDeleted = !IsDeleted;
        }
        public void Modified()
        {
            LastModified = DateTimeOffset.UtcNow;
        }

        public void SetCreatedBy(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new DomainException("User ID cannot be empty");
            CreatedBy = createdBy;
            Modified();
        }
    }
}
