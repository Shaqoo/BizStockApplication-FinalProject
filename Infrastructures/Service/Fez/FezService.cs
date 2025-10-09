using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Service;
using Infrastructures.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructures.Service.Fez
{
    public class FezService : IFezService
    {
        private readonly HttpClient _httpClient;
        private readonly FezSettings _options;
        private readonly JsonSerializerOptions _serializerOptions;
        private string? _authToken;
        private DateTime _expireAt;
        private string? _orgSecretKey;

        public FezService(HttpClient httpClient, IOptions<FezSettings> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private async Task EnsureAuthHeadersAsync()
        {
            var token = await GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            _httpClient.DefaultRequestHeaders.Add("secret-key", _orgSecretKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_authToken) && _expireAt > DateTime.UtcNow)
                return _authToken;

            var response = await _httpClient.PostAsJsonAsync("user/authenticate", new
            {
                user_id = _options.UserId,
                password = _options.Password
            });

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            _authToken = result!.AuthDetails.AuthToken;
            _expireAt = DateTime.Parse(result.AuthDetails.ExpireToken);
            _orgSecretKey = result.OrgDetails.SecretKey;

            return _authToken;
        }

        public async Task<FezResponse<CreateFezOrderResponseDto>> CreateOrderAsync(List<CreateFezOrderRequestItem> requestItems)
        {
            await EnsureAuthHeadersAsync();

            var json = JsonSerializer.Serialize(requestItems);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("order", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new FezResponse<CreateFezOrderResponseDto>
                {
                    Success = false,
                    Message = responseString
                };

            using var doc = JsonDocument.Parse(responseString);
            var status = doc.RootElement.GetProperty("status").GetString() ?? "";
            var message = doc.RootElement.GetProperty("description").GetString() ?? "";
            var orderNos = new Dictionary<string, string>();

            if (doc.RootElement.TryGetProperty("orderNos", out var orderNosElement))
            {
                foreach (var prop in orderNosElement.EnumerateObject())
                    orderNos.Add(prop.Name, prop.Value.GetString() ?? string.Empty);
            }

            return new FezResponse<CreateFezOrderResponseDto>
            {
                Success = status.Equals("Success", StringComparison.OrdinalIgnoreCase),
                Message = message,
                Data = new CreateFezOrderResponseDto
                {
                    Success = status.Equals("Success", StringComparison.OrdinalIgnoreCase),
                    Message = message,
                    OrderNos = orderNos
                }
            };
        }

        public async Task<FezResponse<List<FezOrderSummaryDto>>> GetOrdersByStatusAsync(
    DateTime startDate,
    DateTime endDate)
        {
            await EnsureAuthHeadersAsync();

            var payload = new
            {
                startDate = startDate.ToString("yyyy-MM-dd"),
                endDate = endDate.ToString("yyyy-MM-dd")
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("orders/search", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new FezResponse<List<FezOrderSummaryDto>>
                {
                    Success = false,
                    Message = responseString
                };
            }

            var result = JsonSerializer.Deserialize<FezOrderListResponse>(responseString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var orders = result?.Orders?.Data ?? new List<FezOrderSummaryDto>();

            return new FezResponse<List<FezOrderSummaryDto>>
            {
                Success = true,
                Message = result?.Description ?? "Orders fetched successfully",
                Data = orders
            };
        }



        public async Task<FezResponse<CostEstimateResponseDto>> GetCostAsync(CostEstimateRequestDto request)
        {
            await EnsureAuthHeadersAsync(); 

            var options = new JsonSerializerOptions { PropertyNamingPolicy = null };
            var json = JsonSerializer.Serialize(request, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

             
            var response = await _httpClient.PostAsync("order/cost", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new FezResponse<CostEstimateResponseDto>
                {
                    Success = false,
                    Message = responseString
                };
            }

            var result = JsonSerializer.Deserialize<CostEstimateResponseDto>(responseString, _serializerOptions);

            return new FezResponse<CostEstimateResponseDto>
            {
                Success = true,
                Data = result,
                Message = "OK"
            };
        }


        public async Task<FezResponse<CheckOrderStatusResponseDto>> GetOrderStatusAsync(CheckOrderStatusRequestDto request)
            => await GetAsync<CheckOrderStatusResponseDto>($"orders/{request.OrderNumber}/status");

        public async Task<FezResponse<IEnumerable<CheckOrderStatusResponseDto>>> GetAllOrdersAsync()
            => await GetAsync<IEnumerable<CheckOrderStatusResponseDto>>("orders");

        public async Task<FezResponse<bool>> CancelOrderAsync(string orderNumber)
        {
            await EnsureAuthHeadersAsync();  

            var requestBody = new
            {
                orderNo = orderNumber
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );
            var request = new HttpRequestMessage(HttpMethod.Delete, "order")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new FezResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to cancel order: {responseString}"
                };
            }

            return new FezResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "Order cancelled successfully"
            };
        }


        private async Task<FezResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            await EnsureAuthHeadersAsync();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new FezResponse<TResponse> { Success = false, Message = responseString };

            var result = JsonSerializer.Deserialize<TResponse>(responseString, _serializerOptions);
            return new FezResponse<TResponse> { Success = true, Data = result, Message = "OK" };
        }

        private async Task<FezResponse<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request)
        {
            await EnsureAuthHeadersAsync();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new FezResponse<TResponse> { Success = false, Message = responseString };

            var result = JsonSerializer.Deserialize<TResponse>(responseString, _serializerOptions);
            return new FezResponse<TResponse> { Success = true, Data = result, Message = "OK" };
        }
        private async Task<FezResponse<TResponse>> GetAsync<TResponse>(string url)
        {
            await EnsureAuthHeadersAsync();

            var response = await _httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new FezResponse<TResponse> { Success = false, Message = responseString };

            var result = JsonSerializer.Deserialize<TResponse>(responseString, _serializerOptions);
            return new FezResponse<TResponse> { Success = true, Data = result, Message = "OK" };
        }

        public async Task<FezResponse<DeliveryTimeEstimateResponseDto>> GetDeliveryTimeEstimateAsync(DeliveryTimeEstimateRequestDto request)
        {
            await EnsureAuthHeadersAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(request, options);
            Console.WriteLine("Outgoing JSON: " + json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("delivery-time-estimate", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new FezResponse<DeliveryTimeEstimateResponseDto>
                {
                    Success = false,
                    Message = responseString
                };
            }

            using var doc = JsonDocument.Parse(responseString);
            var eta = doc.RootElement.GetProperty("data").GetProperty("eta").GetString() ?? "";

            return new FezResponse<DeliveryTimeEstimateResponseDto>
            {
                Success = true,
                Data = new DeliveryTimeEstimateResponseDto
                {
                    ETA = eta,
                    Success = true,
                    Message = "OK"
                },
                Message = "OK"
            };
        }


        public async Task<FezResponse<TrackOrderResponseDto>> TrackOrderAsync(string orderNumber)
        {
            await EnsureAuthHeadersAsync();

            var response = await _httpClient.GetAsync($"order/track/{orderNumber}");
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return new FezResponse<TrackOrderResponseDto>
                { Success = false, Message = responseString };

            var doc = JsonSerializer.Deserialize<JsonDocument>(responseString, _serializerOptions)!;
            var orderElement = doc.RootElement.GetProperty("order");
            var historyElement = doc.RootElement.GetProperty("history");

            var result = new TrackOrderResponseDto
            {
                Success = true,
                OrderNumber = orderElement.GetProperty("orderNo").GetString() ?? string.Empty,
                Status = orderElement.GetProperty("orderStatus").GetString() ?? string.Empty,
                RecipientName = orderElement.GetProperty("recipientName").GetString() ?? string.Empty,
                RecipientAddress = orderElement.GetProperty("recipientAddress").GetString() ?? string.Empty,
                SenderName = orderElement.GetProperty("senderName").GetString() ?? string.Empty,
                SenderAddress = orderElement.GetProperty("senderAddress").GetString() ?? string.Empty,
                RecipientState = orderElement.GetProperty("recipientState").GetString() ?? string.Empty,
                CreatedAt = DateTime.Parse(orderElement.GetProperty("createdAt").GetString() ?? DateTime.MinValue.ToString()),
                History = historyElement.EnumerateArray().Select(h => new OrderHistoryDto
                {
                    OrderStatus = h.GetProperty("orderStatus").GetString() ?? string.Empty,
                    StatusCreationDate = DateTime.Parse(h.GetProperty("statusCreationDate").GetString() ?? DateTime.MinValue.ToString()),
                    StatusDescription = h.GetProperty("statusDescription").GetString() ?? string.Empty
                }).ToList()
            };

            return new FezResponse<TrackOrderResponseDto>
            { Success = true, Data = result, Message = "OK" };
        }


    }

}
