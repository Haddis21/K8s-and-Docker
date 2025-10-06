using FoodOrderApp.Models;

namespace FoodOrderApp.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserProfileAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
    }

}
