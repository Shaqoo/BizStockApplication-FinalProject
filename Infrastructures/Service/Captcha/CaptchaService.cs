using Application.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructures.Service.Captcha
{
    public class CaptchaService(IHttpClientFactory clientFactory,IConfiguration configuration) : ICaptchaService
    {
        public async Task<bool> ValidateTokenAsync(string token)
        {
            var secret = configuration["Captcha:Secret"];
            var client = clientFactory.CreateClient();

            var response = await client.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}",null);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CaptchaVerificationResponse>(content);
            return result?.Success == true;

        }

        private record CaptchaVerificationResponse(bool Success, float Score, string[] ErrorCodes);
    }
}
