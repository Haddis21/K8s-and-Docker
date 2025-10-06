using Microsoft.AspNetCore.Components.Authorization;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using FoodOrderApp.Models;


namespace FoodOrderApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);

                if (authResponse != null)
                {
                    await _localStorage.SetItemAsync("authToken", authResponse.Token);
                    await _localStorage.SetItemAsync("currentUser", authResponse.User);
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.Token);
                }

                return authResponse;
            }

            return null;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            var json = JsonSerializer.Serialize(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);

                if (authResponse != null)
                {
                    await _localStorage.SetItemAsync("authToken", authResponse.Token);
                    await _localStorage.SetItemAsync("currentUser", authResponse.User);
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(authResponse.Token);
                }

                return authResponse;
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("currentUser");
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            return await _localStorage.GetItemAsync<UserDto>("currentUser");
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
