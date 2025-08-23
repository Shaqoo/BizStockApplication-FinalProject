using Domain.Enums;

namespace Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        public Role Role { get; private set; }

        private UserRole() { }

        public UserRole(Guid userId, Role role)
        {
            UserId = userId;
            Role = role;
        }
    }

}
