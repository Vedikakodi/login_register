using System.Threading.Tasks;
using DataStore.Abstraction.Repositories;
using FeatureObjects.Abstraction.Managers;
using FeatureObjects.Abstraction.DTOs;
using DataStore.Abstraction.Models; // Ensure User model is recognized

namespace FeatureObjects.Implementation.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;

            return new UserDto
            {
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task AddUserAsync(UserDto userDto, string hashedPassword)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddUser(user);
        }

        public Task<UserDto> GetUserProfileAsync(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
