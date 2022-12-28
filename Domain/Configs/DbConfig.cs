namespace Domain.Configs
{
    public static class DbConfig
    {
        public static string Server { get; set; } = default!;

        public static string Database { get; set; } = default!;

        public static string UserId { get; set; } = default!;

        public static string Password { get; set; } = default!;

        public static string GetConnectionString()
        { return $"Server={Server};Database={Database};User Id={UserId};Password={Password};"; }
    }
}
