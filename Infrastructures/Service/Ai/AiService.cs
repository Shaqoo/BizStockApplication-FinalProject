using Application.Interfaces.Service;
using Infrastructures.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructures.Service.Ai
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly AiSettings _aiSettings;
        private readonly AIResourcesSettings _aiResourcesSettings;
        private readonly Dictionary<string, bool> _userGreeted = new();
        public AiService(HttpClient httpClient, IOptions<AIResourcesSettings> aiResourcesSettings,IOptions<AiSettings> aiSettings)
        {
            _aiSettings = aiSettings.Value;
            _httpClient = httpClient;
            _aiResourcesSettings = aiResourcesSettings.Value;
        }
        public async Task<string> GetResponseAsync(string userId,string role, string question)
        {
            var (apiKey, baseUrl, model) = ResolveAiProvider(role);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var roleResources = _aiResourcesSettings.Roles.ContainsKey(role)
                ? _aiResourcesSettings.Roles[role] : new RoleResources();

            var resourceText = await LoadResourcesAsync(roleResources.Files,roleResources.Urls);

            var userPrompt = new StringBuilder();

            if (!_userGreeted.ContainsKey(userId))
            {
                userPrompt.AppendLine("Hi, I’m BizStock Assistant 👋. I ONLY answer questions related to BizStock inventory, stock, and business operations.");
                _userGreeted[userId] = true;
            }

            userPrompt.AppendLine();
            userPrompt.AppendLine("Resources:");
            userPrompt.AppendLine(resourceText);
            userPrompt.AppendLine();

            userPrompt.AppendLine("IMPORTANT: If the user's question is NOT related to BizStock inventory, stock, or business operations, DO NOT provide an answer. Reply ONLY:");
            userPrompt.AppendLine("'Sorry, I can only assist with BizStock inventory, stock, and business-related questions.'");

            userPrompt.AppendLine();
            userPrompt.AppendLine("User Question:");
            userPrompt.AppendLine(question);


            var body = new
            {
                model = model,
                messages = new[]
                {
                  new { role = role.ToLower(), content = userPrompt.ToString() }
                }
            };

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/chat/completions", body);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString()!;
        }


        private (string apiKey,string baseUrl,string model) ResolveAiProvider(string role)
        {
            return role switch
            {
                "Admin" or "Manager" => (_aiSettings.Gemma, "https://openrouter.ai/api/v1", "google/gemma-3-27b-it"),
                "Customer" => (_aiSettings.Gemini, "https://generativelanguage.googleapis.com/v1beta", "gemini-1.5-pro"),
                "InventoryManager" => (_aiSettings.Groq, "https://api.groq.com/openai/v1", "llama-3.3-70b-versatile"),
                "Guest" => (_aiSettings.Mistral, "https://openrouter.ai/api/v1", "mistralai/mistral-7b-instruct"),
                _=> throw new InvalidOperationException($"Unsupported role: {role}")
            };
        }

        private IEnumerable<string> ChunkText(string text, int chunkSize = 1000)
        {
            for (int i = 0; i < text.Length; i+= chunkSize)
            {
                yield return text.Substring(i, Math.Min(chunkSize, text.Length - 1));   
            }
        }

        private async Task<string> LoadResourcesAsync(IEnumerable<string> files, IEnumerable<string> urls)
        {
            var sb = new StringBuilder();

            if (files != null)
            {
                foreach (var file in files)
                {
                    var content = await File.ReadAllTextAsync(file);
                    foreach (var chunk in ChunkText(content))
                    {
                        sb.AppendLine($"[FILE CHUNK FROM {Path.GetFileName(file)}]"); 
                        sb.AppendLine(chunk);
                    }
                }
            }

            if (urls != null)
            {
                foreach (var url in urls)
                {
                    try
                    {
                        var content = await _httpClient.GetStringAsync(url);
                        foreach (var chunk in ChunkText(content))
                        {
                            sb.AppendLine($"[URL CHUNK from {url}]");
                            sb.AppendLine(chunk);
                        }
                    }
                    catch
                    {
                        sb.AppendLine($"[Failed to fetch: {url}]");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
