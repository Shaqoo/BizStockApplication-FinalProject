using System.Text.Json.Serialization;

namespace Infrastructures.Settings
{
    public class FezSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = "";
    }

    public class AuthResponse
    {
        public string Status { get; set; } = "";
        public string Description { get; set; } = "";
        public AuthDetails AuthDetails { get; set; } = new();
        public OrgDetails OrgDetails { get; set; } = new();
    }

    public class AuthDetails
    {
        public string AuthToken { get; set; } = "";
        public string ExpireToken { get; set; } = "";
    }
    public class OrgDetails
    {
        [JsonPropertyName("secret-key")]
        public string SecretKey { get; set; } = "";

        [JsonPropertyName("Org Full Name")]
        public string OrgFullName { get; set; } = "";
    }
}
