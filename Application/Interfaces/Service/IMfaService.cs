using Application.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IMfaService
    {
        /// <summary>
        /// Generates A New Totp Secret And QrCode For The User
        /// </summary>
        Task<TwoFactorSetupDto> GenerateSecretAndQrAsync(User user);
        /// <summary>
        /// Verifies The Totp Code Submitted By The User
        /// </summary>
        Task<bool> VerifySecretAsync(User user,string code);
        /// <summary>
        /// Returns The Totp Uri string (otpauth://...) 
        /// </summary>
        string GetOtpAuthUri(User user,string base32Secret);
        Task<TwoFactorSetupDto> ResetMfaAsync(User user);
    }
}
