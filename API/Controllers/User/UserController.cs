using API.Utils.User;
using Domain.DTOs.Result;
using Domain.DTOs.User;
using Domain.Entities.User;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.User
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUtils _userUtils;

        public UserController(IUserRepository userRepository, IUserUtils userUtils)
        {
            _userRepository = userRepository;
            _userUtils = userUtils;
        }

        [HttpPost, Route("register")]
        public async Task<ActionResult<Result<UserEntity>>> Register(
            [FromBody] CreateUserDto createUserDto,
            CancellationToken cancellationToken)
        {
            var response = new Result<UserEntity>();

            if(!ModelState.IsValid)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Invalid model.";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            response.ResponseObject = await _userUtils.AddNewUser(createUserDto, cancellationToken);
            response.StatusCode = StatusCodes.Status201Created;
            response.Message = "User Created Successfully.";

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult<Result<string>>> Login(
            [FromBody] LoginUserDto loginUserDto,
            CancellationToken cancellationToken)
        {
            var response = new Result<string>();

            if(!ModelState.IsValid)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Invalid model.";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            var user = await _userRepository.GetUserByUserName(loginUserDto.UserName, cancellationToken);
            response = _userUtils.AuthenticateUser(user, loginUserDto);

            return StatusCode(response.StatusCode, response);
        }
    }
}
