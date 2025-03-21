using System.Threading.Tasks;
using FeatureObjects.Abstraction.DTOs;

namespace FeatureObjects.Abstraction.Managers
{
    public interface IUserManager
    {
        Task<UserDto> GetUserProfileAsync(int userId);
    }
}
