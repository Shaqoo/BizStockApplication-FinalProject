namespace Infrastructures.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Key { get; set; } = default!;
        public int AccessTokenExpirationMinutes { get; set; } = default!;
        public int RefreshTokenExpirationDays { get; set; } = default!;
    }
}
