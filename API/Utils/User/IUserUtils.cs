using Domain.DTOs.Result;
using Domain.DTOs.User;
using Domain.Entities.User;

namespace API.Utils.User
{
    public interface IUserUtils
    {
        public string GenerateJWT(UserEntity user);

        public Result<string> AuthenticateUser(UserEntity user, LoginUserDto loginUserDto);

        public Task<UserEntity> AddNewUser(CreateUserDto createUserDto, CancellationToken cancellationToken);
    }
}
