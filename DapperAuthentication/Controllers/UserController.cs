using DapperAuthentication.Models;
using DapperAuthentication.Enitities;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;

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

        [HttpGet, Route("getUsers")]
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
