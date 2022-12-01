using DapperAuthentication.Models;
using DapperAuthentication.Enitities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DapperAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost, Route("signUp")]
        public async Task<IActionResult> CreateUser(UserModel userModel)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var newUser = new User
            {
                Email = userModel.Email,
                UserName = userModel.UserName,
                NormalizedEmail = userModel.Email.ToUpper(),
                NormalizedUserName = userModel.UserName.ToUpper(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userModel.Password)
            };
            await connection.ExecuteAsync(@"INSERT INTO [User]
                                            (UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash)
                                            VALUES
                                            (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PasswordHash)", newUser);
            return Ok();
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstAsync<User>("SELECT * FROM [User] WHERE UserName=@UserName", new { UserName = loginModel.UserName });
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash);
            if (!isValidPassword)
                return Unauthorized();

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", "1"),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(6),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return Ok(jwtToken);
        }

        [HttpGet, Route("getUsers")]
        [Authorize]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<User>("SELECT * FROM [User]");
            var usermodels = users.Select(user => new {
                UserName = user.UserName,
                Email = user.Email
            });
            return Ok(usermodels);
        }
    }
}
