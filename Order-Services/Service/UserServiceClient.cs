using Order_Services.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Order_Services.Service
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<UserDto?> GetUserAsync(int userId, string authToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken.Replace("Bearer ", ""));

                var response = await _httpClient.GetAsync($"/api/users/{userId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error calling User Service: {ex.Message}");
                return null;
            }
        }
    }
}
