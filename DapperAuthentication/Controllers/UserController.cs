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

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<User>("SELECT * FROM [User]");
            var usermodels = users.Select(user => new UserModel {
                UserName = user.UserName,
                Email = user.Email
            });
            return Ok(usermodels);
        }
    }
}
