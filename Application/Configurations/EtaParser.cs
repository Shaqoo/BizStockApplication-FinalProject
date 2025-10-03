namespace Application.Configurations
{
    public static class EtaParser
    {
        public static DateTime? ParseEta(string? eta)
        {
            if (string.IsNullOrEmpty(eta))
                return null;

            var parts = eta.Split('-');
            if (parts.Length == 0)
                return null;

            var lastPart = parts[^1].Trim().Split(' ')[0];

            if (int.TryParse(lastPart, out var parsedDate))
            {
                return DateTime.UtcNow.AddDays(parsedDate);
            }
            return null;
        }
    }
}
