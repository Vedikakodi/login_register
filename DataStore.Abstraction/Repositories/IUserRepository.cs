using DataStore.Abstraction.Models;
using System.Threading.Tasks;

namespace DataStore.Abstraction.Repositories
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        Task<User> GetUserByEmail(string email);
    }



}
