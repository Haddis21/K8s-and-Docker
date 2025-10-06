using User_Services.Models;

namespace User_Services.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(UserDto user, IList<string> roles);
    }
}
