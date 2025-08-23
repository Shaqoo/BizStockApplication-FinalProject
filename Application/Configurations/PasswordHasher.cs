using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password,string hashSalt)
        {
            var arrg = new Argon2d(Encoding.UTF8.GetBytes(password))
            {
                Salt = Encoding.UTF8.GetBytes(hashSalt),
                Iterations = 10,
                DegreeOfParallelism = 4,
                MemorySize = 38000
            };
            byte[] bytes = arrg.GetBytes(128);
            return Convert.ToBase64String(bytes);

        }
        public static bool VerifyPassword(string hashedPassword, string password,string salt)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Hashed password and password cannot be null or empty.");
            var hashedInput = PasswordHasher.HashPassword(password,salt);
            return hashedInput == hashedPassword;
        }
    }
}
