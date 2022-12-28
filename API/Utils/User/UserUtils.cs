using Domain.Configs;
using Domain.DTOs.Result;
using Domain.DTOs.User;
using Domain.Entities.User;
using Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Utils.User
{
    public class UserUtils : IUserUtils
    {
        private readonly IUserRepository _userRepository;

        public UserUtils(IUserRepository userRepository) { _userRepository = userRepository; }

        public string GenerateJWT(UserEntity user)
        {
            var issuer = JwtConfig.Issuer;
            var audience = JwtConfig.Audience;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(JwtConfig.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject =
                    new ClaimsIdentity(
                        new[]
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("UserEmail", user.Email)
                    }),
                Expires = DateTime.UtcNow.AddMinutes(JwtConfig.ValidInMinutes),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public Result<string> AuthenticateUser(UserEntity user, LoginUserDto loginUserDto)
        {
            var response = new Result<string>();

            if(user == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "No user with such username exists.";
                response.ResponseObject = null;
                return response;
            }

            if(!BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.PasswordHash))
            {
                response.StatusCode = StatusCodes.Status401Unauthorized;
                response.Message = "Username or password is wrong.";
                response.ResponseObject = null;
                return response;
            }

            response.ResponseObject = GenerateJWT(user);
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "User logged in successfully.";
            return response;
        }

        public async Task<UserEntity> AddNewUser(CreateUserDto createUserDto, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
                NormalizedEmail = createUserDto.Email.ToUpper(),
                NormalizedUserName = createUserDto.UserName.ToUpper(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password)
            };
            return await _userRepository.AddUser(userEntity, cancellationToken);
        }
    }
}
