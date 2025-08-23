using Application.Dto;
using Application.Interfaces.Service;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.DataProtection;
using OtpNet;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Infrastructures.Service.MFA
{
    public class MfaService : IMfaService
    {
        private readonly IDataProtector _protector;

        public MfaService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("MfaSecretProtection");
        }

        public async Task<TwoFactorSetupDto> GenerateSecretAndQrAsync(User user)
        {
             
            var secretBytes = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(secretBytes);

             
            var encrypted = _protector.Protect(base32Secret);
            Console.WriteLine("Encryped Mfa "+encrypted.Length);

            user.UpdateTwoFactorSecret(new TwoFactorSecret(encrypted));

             
            var otpUri = GetOtpAuthUri(user, base32Secret);
            var qrCodeBase64 = GenerateQrCodeBase64(otpUri);

            return await Task.FromResult(new TwoFactorSetupDto
            {
                ManualEntryKey = base32Secret,
                QrCodeImageUrl = $"data:image/png;base64,{qrCodeBase64}"
            });
        }

        public string GetOtpAuthUri(User user, string base32Secret)
        {
            var label = $"{(string)user.Email}";
            var issuer = "BizStockApp";

            return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(label)}" +
                   $"?secret={base32Secret}&issuer={Uri.EscapeDataString(issuer)}&digits=6";
        }

        public async Task<TwoFactorSetupDto> ResetMfaAsync(User user)
        {
            return await GenerateSecretAndQrAsync(user);
        }

        public async Task<bool> VerifySecretAsync(User user, string code)
        {
            if (string.IsNullOrWhiteSpace(user.TwoFactorSecret.ToString()))
                return false;

            string decryptedSecret;

            try
            {
                decryptedSecret = _protector.Unprotect(user.TwoFactorSecret.ToString());
            }
            catch
            {
                return false;  
            }

            var secretBytes = Base32Encoding.ToBytes(decryptedSecret);
            var totp = new Totp(secretBytes);

            return await Task.FromResult
                (totp.VerifyTotp(code, out _, VerificationWindow.RfcSpecifiedNetworkDelay));
        }

        private string GenerateQrCodeBase64(string content)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 4  
                }
            };

            var pixelData = writer.Write(content);

            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb
            );

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
        }

        private string GenerateQrCodeBase64WithLogo(string content, string logoPath)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 4  
                }
            };

            var pixelData = writer.Write(content);

            using var qrBitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = qrBitmap.LockBits(
                new Rectangle(0, 0, qrBitmap.Width, qrBitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppRgb
            );

            try
            {
                Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                qrBitmap.UnlockBits(bitmapData);
            }

             
            using var logo = new Bitmap(logoPath);

             
            int logoSize = qrBitmap.Width / 5;
            int logoX = (qrBitmap.Width - logoSize) / 2;
            int logoY = (qrBitmap.Height - logoSize) / 2;

            using var graphics = Graphics.FromImage(qrBitmap);

             
            var smoothingMode = graphics.SmoothingMode;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.FillEllipse(Brushes.White, logoX, logoY, logoSize, logoSize);
            graphics.SmoothingMode = smoothingMode;

             
            graphics.DrawImage(logo, new Rectangle(logoX, logoY, logoSize, logoSize));

             
            using var ms = new MemoryStream();
            qrBitmap.Save(ms, ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
        }

    }
}
