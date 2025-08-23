using Application.Interfaces.Service.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructures.Service.RecoveryCode
{
  
    public class RecoveryCodeGenerator : IRecoveryCodeGenerator
    {
        public IReadOnlyCollection<string> Generate(int count)
        {
            var codes = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                codes.Add(GenerateCode());
            }

            return codes;
        }

        private string GenerateCode(int length = 12)
        {
             
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = RandomNumberGenerator.GetBytes(length);

            var sb = new StringBuilder(length);
            foreach (var b in data)
            {
                sb.Append(chars[b % chars.Length]);
            }

            return sb.ToString();
        }
    }

}
