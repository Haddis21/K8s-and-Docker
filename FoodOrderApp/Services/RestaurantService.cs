using FoodOrderApp.Models;
using System.Text.Json;

namespace FoodOrderApp.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestaurantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        // public RestaurantService(IHttpClientFactory httpClientFactory)
        // {
        //     _httpClient = httpClientFactory.CreateClient("RestaurantServiceClient");
        //     _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        // }

        public async Task<List<RestaurantDto>> GetRestaurantsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/restaurants");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<RestaurantDto>>(json, _jsonOptions) ?? new List<RestaurantDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching restaurants: {ex.Message}");
            }

            return new List<RestaurantDto>();
        }

        public async Task<RestaurantDto?> GetRestaurantAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<RestaurantDto>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching restaurant: {ex.Message}");
            }

            return null;
        }

        public async Task<List<RestaurantDto>> SearchRestaurantsAsync(string query)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/search?query={Uri.EscapeDataString(query)}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<RestaurantDto>>(json, _jsonOptions) ?? new List<RestaurantDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching restaurants: {ex.Message}");
            }

            return new List<RestaurantDto>();
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync(int restaurantId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/{restaurantId}/menu");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<MenuItemDto>>(json, _jsonOptions) ?? new List<MenuItemDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching menu items: {ex.Message}");
            }

            return new List<MenuItemDto>();
        }
    }
}

