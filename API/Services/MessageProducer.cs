using Domain.Configs;
using Domain.Entities.Message;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace API.Services
{
    public class MessageProducer : IMessageProducer
    {
        private readonly IModel _channel;

        public MessageProducer()
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMqConfig.HostName,
                UserName = RabbitMqConfig.UserName,
                Password = RabbitMqConfig.Password,
                VirtualHost = RabbitMqConfig.VirtualHost
            };

            var connection = factory.CreateConnection();

            _channel = connection.CreateModel();
            _channel.QueueDeclare("message", durable: true, exclusive: false, autoDelete: false);
        }

        public void SendMessage(MessageEntity messageEntity)
        {
            var serializedMessage = JsonSerializer.Serialize(messageEntity);
            var rabbitMqMessageBody = Encoding.UTF8.GetBytes(serializedMessage);

            _channel.BasicPublish(string.Empty, "message", body: rabbitMqMessageBody);
        }
    }
}
