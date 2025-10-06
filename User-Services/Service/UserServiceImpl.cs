using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User_Services.Models;

namespace User_Services.Service
{
    public class UserServiceImpl : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserServiceImpl(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> UpdateProfileAsync(int userId, UpdateProfileDto updateDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            user.FirstName = updateDto.FirstName;
            user.LastName = updateDto.LastName;
            user.PhoneNumber = updateDto.PhoneNumber;

            if (updateDto.Address != null)
            {
                user.Address = new Address
                {
                    Street = updateDto.Address.Street,
                    City = updateDto.Address.City,
                    PostalCode = updateDto.Address.PostalCode,
                    Latitude = updateDto.Address.Latitude,
                    Longitude = updateDto.Address.Longitude
                };
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded ? MapToDto(user) : null;
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Where(u => u.IsActive)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = user.Address != null ? new AddressDto
                {
                    Street = user.Address.Street,
                    City = user.Address.City,
                    PostalCode = user.Address.PostalCode,
                    Latitude = user.Address.Latitude,
                    Longitude = user.Address.Longitude
                } : null,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}
