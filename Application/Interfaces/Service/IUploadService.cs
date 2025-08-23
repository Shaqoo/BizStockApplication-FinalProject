using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Service
{
    public interface IUploadService
    {
        Task<string> UploadProfileImageAsync(Stream fileStream, string fileName);
        Task<string> UploadProductImageAsync(Stream fileStream, string fileName);
        Task<string> MessageImageAsync(Stream fileStream, string fileName);
        Task<string> MessageAudioAsync(Stream fileStream, string fileName);

    }
}
