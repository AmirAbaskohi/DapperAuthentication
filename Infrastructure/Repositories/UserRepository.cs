using Dapper;
using Domain.Configs;
using Domain.Entities.User;
using Domain.Repositories;
using System.Data.SqlClient;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<UserEntity> AddUser(UserEntity userEntity, CancellationToken cancellationToken)
        {
            using(var connection = new SqlConnection(DbConfig.GetConnectionString()))
            {
                connection.Open();

                var insertUserSql = @"INSERT INTO [AmirhosseinUser]
                                      (UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash)
                                      OUTPUT INSERTED.*
                                      VALUES
                                      (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @PasswordHash)";

                return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                    new CommandDefinition(insertUserSql, userEntity, cancellationToken: cancellationToken));
            }
        }

        public async Task<UserEntity> GetUserByUserName(string userName, CancellationToken cancellationToken)
        {
            using(var connection = new SqlConnection(DbConfig.GetConnectionString()))
            {
                connection.Open();

                var getUserByUserName = @"SELECT * FROM [AmirhosseinUser] WHERE UserName=@UserName";

                return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                    new CommandDefinition(
                        getUserByUserName,
                        new { UserName = userName },
                        cancellationToken: cancellationToken));
            }
        }
    }
}
