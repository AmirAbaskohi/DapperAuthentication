using Domain.Entities.Message;

namespace Domain.Repositories
{
    public interface IMessageRepository
    {
        public MessageEntity AddMessage(MessageEntity messageEntity, CancellationToken cancellationToken);

        public IEnumerable<MessageEntity> RunChangeMessageStatusSP(CancellationToken cancellationToken);
    }
}
