using FoodOrderApp.Models;

namespace FoodOrderApp.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync();
        Task<UserDto?> GetCurrentUserAsync();
        Task<string?> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
