using Domain.Configs;
using Domain.Entities.Message;
using Domain.Enums;
using Domain.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Console.Services
{
    public class MessageConsumer
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IModel _channel;

        public MessageConsumer(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

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

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                System.Console.WriteLine("New Message Started To Be Received");
                var rabbitMqMessageBody = eventArgs.Body.ToArray();
                var serializedMessage = Encoding.UTF8.GetString(rabbitMqMessageBody);
                var messageEntity = JsonSerializer.Deserialize<MessageEntity>(serializedMessage);
                messageEntity.Status = (byte)MessageStatusEnum.MessageStatus.Created;
                messageEntity.CreatedOn = DateTime.UtcNow;
                messageEntity = _messageRepository.AddMessage(messageEntity, CancellationToken.None);
                System.Console.WriteLine("New Message Received");
            };

            _channel.BasicConsume("message", true, consumer);
        }
    }
}
