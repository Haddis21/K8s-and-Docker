using Order_Services.Models;
using System.Text.Json;

namespace Order_Services.Service
{
    public class RestaurantServiceClient : IRestaurantServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public RestaurantServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<RestaurantDto?> GetRestaurantAsync(int restaurantId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/{restaurantId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RestaurantDto>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Restaurant Service: {ex.Message}");
                return null;
            }
        }

        public async Task<MenuItemDto?> GetMenuItemAsync(int restaurantId, int menuItemId)
        {
            try
            {
                var menuItems = await GetMenuItemsAsync(restaurantId);
                return menuItems.FirstOrDefault(m => m.Id == menuItemId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting menu item: {ex.Message}");
                return null;
            }
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync(int restaurantId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/restaurants/{restaurantId}/menu");

                if (!response.IsSuccessStatusCode)
                    return new List<MenuItemDto>();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<MenuItemDto>>(json, _jsonOptions) ?? new List<MenuItemDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting menu items: {ex.Message}");
                return new List<MenuItemDto>();
            }
        }
    }
}
