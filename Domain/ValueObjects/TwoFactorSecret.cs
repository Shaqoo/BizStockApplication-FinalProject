using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record TwoFactorSecret
    {
        public string Secret { get; }

        public TwoFactorSecret(string secret)
        {
            Secret = Validate(secret);
        }

        public static TwoFactorSecret Create(string secret) => new(Validate(secret));

        private static string Validate(string secret)
        {
            if (string.IsNullOrWhiteSpace(secret) || secret.Length < 16)
                throw new DomainException("Invalid 2FA secret.");
            return secret;
        }

        public override string ToString() => Secret;

        public static explicit operator string(TwoFactorSecret s) => s.Secret;
    }


}
