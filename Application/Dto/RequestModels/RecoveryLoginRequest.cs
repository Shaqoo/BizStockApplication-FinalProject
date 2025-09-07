using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class RecoveryLoginRequest
    {
        public string Email { get; set; }

        public string RecoveryCode { get; set; }

        public string TempToken { get; set; }
        public RecoveryLoginRequest(string email, string recoveryCode,string tempToken)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required", nameof(email));
            if (string.IsNullOrWhiteSpace(recoveryCode)) throw new ArgumentException("Recovery code is required", nameof(recoveryCode));
            if (string.IsNullOrWhiteSpace(tempToken)) throw new ArgumentException("Temporary token is required", nameof(tempToken));
            Email = email;
            RecoveryCode = recoveryCode;
            TempToken = tempToken;  
        }

    }
}
