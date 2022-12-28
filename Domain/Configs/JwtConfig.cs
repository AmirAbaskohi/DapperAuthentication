namespace Domain.Configs
{
    public static class JwtConfig
    {
        public static string Issuer { get; set; } = default!;

        public static string Audience { get; set; } = default!;

        public static string Key { get; set; } = default!;

        public static int ValidInMinutes { get; set; } = default!;
    }
}
