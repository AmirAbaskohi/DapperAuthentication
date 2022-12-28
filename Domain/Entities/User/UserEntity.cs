namespace Domain.Entities.User
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = default!;

        public string NormalizedUserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string NormalizedEmail { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;
    }
}
