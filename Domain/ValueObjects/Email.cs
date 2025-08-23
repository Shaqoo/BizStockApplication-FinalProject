using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record Email
    {
        public string Value { get; } = default!;

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email Cannot Be Empty");

            string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(value, emailRegex, RegexOptions.IgnoreCase))
                throw new DomainException("Invalid email address.");
            Value = value;
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email Cannot Be Empty");
            string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(value, emailRegex, RegexOptions.IgnoreCase))
                throw new DomainException("Invalid email address.");
            return new Email(value);
        }


        public static explicit operator string(Email email) => email.Value;
        
    }

}
