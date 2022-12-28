using API.Services;
using Domain.DTOs.Message;
using Domain.DTOs.Result;
using Domain.Entities.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Message
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public MessageController(IMessageProducer messageProducer) { _messageProducer = messageProducer; }

        [HttpPost]
        public ActionResult<Result> SendMessage(MessageDto messageDto)
        {
            var response = new Result();

            if(!ModelState.IsValid)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "Invalid model.";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            var messageEntity = new MessageEntity
            {
                Title = messageDto.Title,
                Body = messageDto.Body,
                CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value)
            };
            _messageProducer.SendMessage(messageEntity);

            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Message sent successfully.";

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
