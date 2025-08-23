using Application.Interfaces.Service;
using sib_api_v3_sdk.Model;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Task = System.Threading.Tasks.Task;
using Microsoft.Extensions.Configuration;

namespace Infrastructures.Service.Email
{
    public class MailjetNotificationService : IEmailNotificationService
    {
        private readonly IConfiguration _configuration;

        public MailjetNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string content, List<SendSmtpEmailAttachment>? attachments = null)
        {
            var client = new HttpClient();

            var publicKey = _configuration["Mailjet:ApiKey"];
            var privateKey = _configuration["Mailjet:PrivateKey"];

            var byteArray = Encoding.ASCII.GetBytes($"{publicKey}:{privateKey}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var htmlTemplate = await File.ReadAllTextAsync("C:\\Users\\ADMIN\\source\\repos\\BizStockApplication\\Infrastructures\\Service\\Email\\Email.html");

            var messageHtml = htmlTemplate
                .Replace("{{content}}", content)
                .Replace("{{year}}", DateTime.UtcNow.Year.ToString());


            var messageBody = new
            {
                Messages = new[]
                {
                new
                {
                    From = new { Email = "shakirullahohio@gmail.com", Name = "BizStock" },
                    To = new[] { new { Email = to } },
                    Subject = subject,
                    HTMLPart = messageHtml
                }
            }
            };

            var json = JsonSerializer.Serialize(messageBody);
            var response = await client.PostAsync(
                "https://api.mailjet.com/v3.1/send",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Raw response from Mailjet:");
                Console.WriteLine(responseBody);

                throw new Exception($"Mailjet error: {response.StatusCode} - {responseBody}");
            }
        }
    }

}
