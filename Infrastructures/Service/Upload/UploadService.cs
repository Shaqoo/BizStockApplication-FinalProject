using Application.Dto;
using Application.Interfaces.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructures.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Infrastructures.Service.Upload
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadService(IOptions<CloudinarySettings> cloudinarySettings,IWebHostEnvironment webHostEnvironment)
        {
             _webHostEnvironment = webHostEnvironment;

            var settings = cloudinarySettings.Value;
            
            Console.WriteLine(settings.CloudName);
            Console.WriteLine(settings.ApiKey);
            Console.WriteLine(settings.ApiSecret);
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(cloudinarySettings), "Cloudinary settings cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(settings.CloudName) ||
                string.IsNullOrWhiteSpace(settings.ApiKey) ||
                string.IsNullOrWhiteSpace(settings.ApiSecret))
            {
                Console.WriteLine(settings.CloudName);
                Console.WriteLine(settings.ApiKey);
                Console.WriteLine(settings.ApiSecret);
                throw new ArgumentException("Cloudinary configuration is missing required values.");
            }

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            _cloudinary = new Cloudinary(account)
            {
                Api = { Secure = true }
            };
        }

        public async Task<string> UploadProfileImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "bizstock-user-profile-images",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true,
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Error uploading image: {uploadResult.Error.Message}");
            }
            return uploadResult.SecureUri.ToString() ?? throw new Exception("Cloudinary upload failed");
        }
        public async Task<string> UploadProductImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "bizstock-product-images",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true,
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Error uploading image: {uploadResult.Error.Message}");
            }
            return uploadResult.SecureUri.ToString() ?? throw new Exception("Cloudinary upload failed");
        }

        public async Task<string> MessageImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "bizstock-message-images",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true,
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Error uploading image: {uploadResult.Error.Message}");
            }
            return uploadResult.SecureUri.ToString() ?? throw new Exception("Cloudinary upload failed");
        }

        public async Task<string> MessageAudioAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "bizstock-message-audio",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception($"Error uploading audio: {uploadResult.Error.Message}");

            return uploadResult.SecureUrl?.ToString()
                ?? throw new Exception("Cloudinary upload failed");
        }

        public async Task<Result<string>> UploadQrCodeAsync(IFormFile QrCode)
        {
             if(QrCode == null || QrCode.Length == 0)
                return Result<string>.Failure("No file uploaded.");

             var qrCodeFolder = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/qrcodes");
            if (!Directory.Exists(qrCodeFolder))
            {
                Directory.CreateDirectory(qrCodeFolder);
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(QrCode.FileName)}";
            var filePath = Path.Combine(qrCodeFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await QrCode.CopyToAsync(stream);
            }
            var qrCodeUrl = $"/qrcodes/{fileName}";

            return Result<string>.Success(qrCodeUrl, "QR Code Uploaded Successfully");
        }


        public async Task<Result<string>> UploadQrCodeAsync(string qrPayload)
        {
            try
            {
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        Height = 300,
                        Width = 300,
                        Margin = 1
                    }
                };
             
                var pixelData = writer.Write(qrPayload);

                using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
                var bitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format32bppRgb);

                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                string qrFolder = Path.Combine(_webHostEnvironment.WebRootPath, "qrcodes");
                Directory.CreateDirectory(qrFolder);

                string qrFileName = $"{Guid.NewGuid()}.png";
                string qrPath = Path.Combine(qrFolder, qrFileName);

                bitmap.Save(qrPath, ImageFormat.Png);

                string qrUrl = $"/qrcodes/{qrFileName}";
                return Result<string>.Success(qrUrl);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error generating QR code: {ex.Message}");
            }
        }
    }
}
