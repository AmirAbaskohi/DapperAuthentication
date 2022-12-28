using Dapper;
using Domain.Configs;
using Domain.Entities.Message;
using Domain.Repositories;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public MessageEntity AddMessage(MessageEntity messageEntity, CancellationToken cancellationToken)
        {
            using(var connection = new SqlConnection(DbConfig.GetConnectionString()))
            {
                connection.Open();

                var insertMesssageSql = @"INSERT INTO [AmirhosseinMessage]
                                          (Title, Body, Status, CreatedBy, CreatedOn)
                                          OUTPUT INSERTED.*
                                          VALUES
                                          (@Title, @Body, @Status, @CreatedBy, @CreatedOn)";

                return connection.QueryFirstOrDefault<MessageEntity>(
                    new CommandDefinition(insertMesssageSql, messageEntity, cancellationToken: cancellationToken));
            }
        }

        public IEnumerable<MessageEntity> RunChangeMessageStatusSP(CancellationToken cancellationToken)
        {
            using(var connection = new SqlConnection(DbConfig.GetConnectionString()))
            {
                connection.Open();

                return connection.Query<MessageEntity>(
                    new CommandDefinition(
                        "ChangeMessageStatus",
                        commandType: System.Data.CommandType.StoredProcedure,
                        cancellationToken: cancellationToken));
            }
        }
    }
}
