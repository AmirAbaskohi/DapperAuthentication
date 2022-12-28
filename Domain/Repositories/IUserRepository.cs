using Domain.Entities.User;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<UserEntity> AddUser(UserEntity userEntity, CancellationToken cancellationToken);

        public Task<UserEntity> GetUserByUserName(string userName, CancellationToken cancellationToken);
    }
}
