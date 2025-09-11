using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using NpgsqlTypes;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public Email Email { get; private set; } = default!;
        public string FullName { get; private set; } = default!;
        public string Password { get; private set; } = default!;
        public string HashSalt { get; private set; } = default!;
        public TwoFactorSecret? TwoFactorSecret { get; private set; } = default!;
        public bool IsTwoFactorEnabled { get; private set; } = true;
        public bool IsEmailVerified { get; private set; } = false;
        public PhoneNumber PhoneNumber { get; private set; } = default!;
        public bool IsPhoneNumberVerified { get; private set; } = false;
        public Gender Gender { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; } = default!;
        public string? ProfilePictureUrl { get; private set; }
        public Wallet? Wallet { get; private set; } = default!;
        public Guid? WalletId { get; private set; }
        public ICollection<Notification> Notifications { get; private set; } = new HashSet<Notification>();
        public ICollection<ChatMessage> ChatMessages { get; private set; } = new HashSet<ChatMessage>();
        public ICollection<MessageReaction> Reactions { get; private set; } = new HashSet<MessageReaction>();
        public ICollection<Payment> Payments { get; private set; } = new HashSet<Payment>();

        private readonly HashSet<FidoCredential> _fidoCredentials = new();
        public IReadOnlyCollection<FidoCredential> FidoCredentials => _fidoCredentials;

        private readonly HashSet<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;
        public DateTime LastLoggedIn { get; private set; } = DateTime.UtcNow;
        public string RefreshToken { get;private set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get;private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockoutEnd { get; private set; }
        public NpgsqlTsVector SearchVector { get; private set; } = default!;
        private readonly HashSet<UserRecoveryCode> _recoveryCodes = new();
        public IReadOnlyCollection<UserRecoveryCode> RecoveryCodes => _recoveryCodes;
        public bool RequiresCaptcha => FailedLoginAttempts >= 3 && !IsLockedOut;


        public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow;

        private User() { }

        public User(Email email, string password, string salt, PhoneNumber phoneNumber, Gender gender, DateOfBirth dob,string fullName)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new DomainException("Password is required");
            if (string.IsNullOrWhiteSpace(salt)) throw new DomainException("Salt is required");
            if (string.IsNullOrWhiteSpace(fullName)) throw new DomainException("FullName is required");
            Email = email;
            Password = password;
            HashSalt = salt;
            PhoneNumber = phoneNumber;
            Gender = gender;
            DateOfBirth = dob;
            FullName = fullName;
        }

        public void VerifyEmail()
        { 
            IsEmailVerified = true;
            Modified();
        }

        public void VerifyPhoneNumber()
        {
            IsPhoneNumberVerified = true;
            Modified();
        }
        public void LatestLogin()
        {
            LastLoggedIn = DateTime.UtcNow;
            Modified();
        }
        public void AddRefreshToken(string token, DateTime expiry)
        {
            if (string.IsNullOrWhiteSpace(token)) 
                throw new DomainException("Refresh token is required");
            RefreshToken = token;
            RefreshTokenExpiryTime = expiry;
            Modified();
        }
        public void AddFidoCredential(FidoCredential credential)
        {
            if (credential == null) 
                throw new DomainException(nameof(credential));

            _fidoCredentials.Add(credential);
            Modified();
        }

        public void RemoveFidoCredential(Guid credentialId)
        {
            var cred = _fidoCredentials.FirstOrDefault(c => c.Id == credentialId);
            if (cred != null)
                _fidoCredentials.Remove(cred);
            Modified();
        }

        public void ChangePassword(string newPassword, string newSalt)
        {
            if (string.IsNullOrWhiteSpace(newPassword)) 
                throw new DomainException("Password is required");

            Password = newPassword;
            HashSalt = newSalt;
            Modified();
        }
        public bool VerifyPassword(string password)
        {
            return Password.Equals(password);
        }

        public void UpdateTwoFactorSecret(TwoFactorSecret secret)
        {
            IsTwoFactorEnabled = true;
            TwoFactorSecret = secret ?? throw new ArgumentNullException(nameof(secret));
            Modified();
        }

        public void AddRole(Role role)
        {
            if (!_userRoles.Any(r => r.Role == role))
                _userRoles.Add(new UserRole(this.Id, role));
        }

        public void RemoveRole(Role role)
        {
            var toRemove = _userRoles.FirstOrDefault(r => r.Role == role);
            if (toRemove != null)
                _userRoles.Remove(toRemove);
            Modified();
        }

        public bool HasRole(Role role) => _userRoles.Any(r => r.Role == role);

        public void UpdateProfilePicture(string pictureUrl)
        {
            if (string.IsNullOrWhiteSpace(pictureUrl))
                throw new DomainException("Invalid picture URL");

            ProfilePictureUrl = pictureUrl;
            Modified();
        }

        public void ChangeName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new DomainException("FullName is required");
            FullName = fullName;
            Modified();
        }
        public void RegisterFailedLogin()
        {
            FailedLoginAttempts++;
            if (FailedLoginAttempts >= 5)
            {
                LockoutEnd = DateTime.UtcNow.AddMinutes(10);  
            }
        }

        public void ResetLoginAttempts()
        {
            FailedLoginAttempts = 0;
            LockoutEnd = null;
        }

        public void EnableTwoFactor(TwoFactorSecret secret, IEnumerable<UserRecoveryCode> recoveryCodes)
        {
            TwoFactorSecret = secret ?? throw new DomainException(nameof(secret));
            IsTwoFactorEnabled = true;

            _recoveryCodes.Clear();
            foreach (var code in recoveryCodes)
                _recoveryCodes.Add(code);
        }
        public void ClearRecoveryCodes()
        {
            _recoveryCodes.Clear();
        }
        public void AddRecoveryCode(UserRecoveryCode recoveryCode)
        {
            if (recoveryCode == null) 
                throw new DomainException(nameof(recoveryCode));
            _recoveryCodes.Add(recoveryCode);
        }

        public void DisableTwoFactor()
        {
            TwoFactorSecret = null;
            IsTwoFactorEnabled = false;
            _recoveryCodes.Clear();
        }

        public bool UseRecoveryCode(string code)
        {
            var recoveryCode = _recoveryCodes.FirstOrDefault(c => c.Code == code);
            if (recoveryCode == null || recoveryCode.IsUsed)
                return false;

            recoveryCode.MarkAsUsed();
            return true;
        }
    }
}
