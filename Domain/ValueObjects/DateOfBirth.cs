using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public record DateOfBirth
    {
        public DateTime Value { get; }

        public DateOfBirth(DateTime value)
        {
            //if (value.Kind != DateTimeKind.Unspecified)
            //    throw new DomainException("DOB should have unspecified DateTimeKind.");

            if (value > DateTime.UtcNow)
                throw new DomainException("Date of birth cannot be in the future.");

            var minimumAllowed = DateTime.UtcNow.AddYears(-18);
            if (value > minimumAllowed)
                throw new DomainException("User must be at least 18 years old.");

            Value = value.Date;
        }


        public static DateOfBirth Create(DateTime value)
        {
            //if (value.Kind != DateTimeKind.Unspecified)
            //    throw new DomainException("DOB should have unspecified DateTimeKind.");

            if (value > DateTime.UtcNow)
                throw new DomainException("Date of birth cannot be in the future.");

            var minimumAllowed = DateTime.UtcNow.AddYears(-18);
            if (value > minimumAllowed)
                throw new DomainException("User must be at least 18 years old.");

            return new DateOfBirth(value);
        }


        public int Age => (int)((DateTime.UtcNow - Value).TotalDays / 365.25);
        public override string ToString() => Value.ToString("yyyy-MM-dd");
        public static explicit operator DateTime(DateOfBirth dob) => dob.Value;
    }

}
