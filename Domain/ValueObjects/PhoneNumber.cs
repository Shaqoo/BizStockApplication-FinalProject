using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    using Domain.Exceptions;
    using System.Text.RegularExpressions;

    public record PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Phone number is required.");

            var phoneRegex = @"^\+?[1-9]\d{7,14}$";
            if (!Regex.IsMatch(value, phoneRegex))
                throw new DomainException("Invalid phone number format.");
            Value = value;
        }

        public static PhoneNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Phone number is required.");

            var phoneRegex = @"^\+?[1-9]\d{7,14}$";  
            if (!Regex.IsMatch(value, phoneRegex))
                throw new DomainException("Invalid phone number format.");

            return new PhoneNumber(value);
        }

        public override string ToString() => Value;
        public static explicit operator string(PhoneNumber phone) => phone.Value;
    }

}
