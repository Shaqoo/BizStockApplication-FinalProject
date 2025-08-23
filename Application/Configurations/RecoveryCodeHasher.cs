namespace Application.Configurations
{
    public static class RecoveryCodeHasher
    {
        public static string Hash(string code)
        {
            return BCrypt.Net.BCrypt.HashPassword(code);
        }

        public static bool Verify(string code, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(code, hash);
        }
    }

}
