using BikeRentalManagement.Models;
using BikeRentalManagement.Repositories;

namespace BikeRentalManagement.Services
{
    // File: Services/UserService.cs
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Login(string username, string password, string role)
        {
            return _userRepository.GetUser(username, password, role);
        }

        public bool Register(User user)
        {
            if (_userRepository.GetUser(user.UserName, user.Password, user.Role) != null)
            {
                // User with the same username already exists
                return false;
            }
            return _userRepository.RegisterUser(user);
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }

}
