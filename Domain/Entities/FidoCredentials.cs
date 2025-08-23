using Domain.Exceptions;

namespace Domain.Entities
{

    public class FidoCredential
    {
        public Guid Id { get; private init; }

        public Guid UserId { get; private set; }

        public User User { get; private set; } = default!;

        public string CredentialId { get; private set; } = default!;

        public string PublicKey { get; private set; } = default!;

        public uint SignatureCounter { get; private set; }
        public DateTimeOffset CreatedAt { get; private init; }
        public Guid AuthenticatorAAGUID { get; private set; }


        private FidoCredential() { }

        public FidoCredential(
            Guid userId,
            string credentialId,
            string publicKey,
            Guid authenticatorAAGUID,
            uint signatureCounter = 0)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CredentialId = !string.IsNullOrWhiteSpace(credentialId) ? credentialId : throw new DomainException("Credential ID cannot be empty.");
            PublicKey = !string.IsNullOrWhiteSpace(publicKey) ? publicKey : throw new DomainException("Public key cannot be empty.");
            AuthenticatorAAGUID = authenticatorAAGUID;
            SignatureCounter = signatureCounter;
            CreatedAt = DateTimeOffset.Now;
        }

        public void UpdateSignatureCounter(uint newCounter)
        {
            if (newCounter > SignatureCounter)
                SignatureCounter = newCounter;
        }
    }
}
