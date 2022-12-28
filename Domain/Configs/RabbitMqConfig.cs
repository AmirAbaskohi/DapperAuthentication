namespace Domain.Configs
{
    public static class RabbitMqConfig
    {
        public static string HostName { get; set; } = default!;

        public static string UserName { get; set; } = default!;

        public static string Password { get; set; } = default!;

        public static string VirtualHost { get; set; } = default!;
    }
}
