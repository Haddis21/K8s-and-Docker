using FoodOrderApp.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodOrderApp.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<UserDto?> GetUserProfileAsync()
        {
            try
            {
                await SetAuthorizationHeader();

                var response = await _httpClient.GetAsync("/api/users/profile");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user profile: {ex.Message}");
            }

            return null;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            try
            {
                await SetAuthorizationHeader();

                var response = await _httpClient.GetAsync($"/api/users/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
            }

            return null;
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }

}
