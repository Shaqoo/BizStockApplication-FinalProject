using Application.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructures.Service.Captcha
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IConfiguration configuration;

        public CaptchaService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            this.clientFactory = clientFactory;
            this.configuration = configuration;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var secret = configuration["Captcha:Secret"]!;
            var client = clientFactory.CreateClient();

            var values = new Dictionary<string, string>
            {
                { "secret", secret?.Trim() ?? "" },
                { "response", token?.Trim() ?? "" }
            };

            Console.WriteLine(secret);

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            var result = JsonSerializer.Deserialize<CaptchaVerificationResponse>(responseBody);
            
            return result?.Success == true;
        }

        private record CaptchaVerificationResponse(bool Success, float Score, string[] ErrorCodes);

    }

}
