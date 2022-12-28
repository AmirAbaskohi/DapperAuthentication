using Domain.Entities.Message;

namespace API.Services
{
    public interface IMessageProducer
    {
        public void SendMessage(MessageEntity messageEntity);
    }
}
