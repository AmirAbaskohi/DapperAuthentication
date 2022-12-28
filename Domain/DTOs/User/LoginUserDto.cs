namespace Domain.DTOs.User
{
    public class LoginUserDto
    {
        public string UserName { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
