using Domain.Entities;

namespace Domain.Users
{
    public interface IUserRepository
    {
        Task<User> Get(string userName);  
    }
}
