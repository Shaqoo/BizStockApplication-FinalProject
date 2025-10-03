using Application.Dto;
using Application.Interfaces.Service;
using Infrastructures.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Infrastructures.Service.Payments
{
    public class PaystackPaymentGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly PaystackSettings _settings;
        private readonly ILogger<PaystackPaymentGatewayService> _logger;

        public PaystackPaymentGatewayService(
            IOptions<PaystackSettings> settings,
            ILogger<PaystackPaymentGatewayService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.paystack.co/")
            };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _settings.SecretKey);
        }

        public async Task<string> InitializeTransactionAsync(decimal amount, string email, string reference)
        {
            var payload = new
            {
                email,
                amount = (int)(amount * 100),  
                reference,
                callback_url = "http://localhost:5500/roles/Customer/Pages/payment-status.html"
            };


            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Initializing Paystack transaction for {Email} with reference {Reference}", email, reference);

            var response = await _httpClient.PostAsync("transaction/initialize", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Paystack InitTransaction failed. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                throw new ApplicationException("Paystack initialization failed.");
            }

            _logger.LogInformation("Paystack transaction initialized successfully for {Reference}", reference);
            return responseBody;
        }

        public async Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference)
        {
            _logger.LogInformation("Verifying Paystack transaction with reference {Reference}", reference);

            var response = await _httpClient.GetAsync($"transaction/verify/{reference}");
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Paystack VerifyTransaction failed. Status: {StatusCode}, Response: {Response}", response.StatusCode, responseBody);
                throw new ApplicationException("Paystack verification failed.");
            }

            var result = JsonConvert.DeserializeObject<PaystackVerifyResponse>(responseBody)
                         ?? throw new ApplicationException("Invalid response from Paystack");

            _logger.LogInformation("Paystack transaction verification successful for {Reference}", reference);
            return result;
        }

    }

}
