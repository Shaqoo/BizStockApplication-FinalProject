using Application.Interfaces.Service;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructures.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructures.Service.Upload
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;

        public UploadService(IOptions<CloudinarySettings> cloudinarySettings)
        {
             

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
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "bizstock-message-audio",
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
    }
}
