using Domain.Entities.User;
using Domain.Enums;

namespace Domain.Entities.Message
{
    public class MessageEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public string Body { get; set; } = default!;

        public byte Status { get; set; }

        public Guid CreatedBy { get; set; }

        public UserEntity CreatedByUserEntity { get; set; } = default!;

        public DateTime CreatedOn { get; set; }

        public override string ToString()
        { return $"Message Id: {Id} || Title: {Title} || Body: {Body} <CreatedOn: {CreatedOn}> <From: {CreatedBy}>"; }
    }
}
