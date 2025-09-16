using Application.Dto;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Service
{
    public interface IUploadService
    {
        Task<string> UploadProfileImageAsync(Stream fileStream, string fileName);
        Task<string> UploadProductImageAsync(Stream fileStream, string fileName);
        Task<string> MessageImageAsync(Stream fileStream, string fileName);
        Task<string> MessageAudioAsync(Stream fileStream, string fileName);
        Task<Result<string>> UploadQrCodeAsync(IFormFile QrCode);
        Task<Result<string>> UploadQrCodeAsync(string qrPayload);

    }
}
