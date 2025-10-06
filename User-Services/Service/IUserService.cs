using User_Services.Models;

namespace User_Services.Service
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto updateDto);
        Task<bool> DeactivateUserAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
