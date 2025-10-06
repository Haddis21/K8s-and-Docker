using FoodOrderApp.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FoodOrderApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public OrderService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                await SetAuthorizationHeader();

                var json = JsonSerializer.Serialize(createOrderDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/order", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OrderDto>(responseContent, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
            }

            return null;
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync()
        {
            try
            {
                await SetAuthorizationHeader();

                var response = await _httpClient.GetAsync("/api/order");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions) ?? new List<OrderDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching orders: {ex.Message}");
            }

            return new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            try
            {
                await SetAuthorizationHeader();

                var response = await _httpClient.GetAsync($"/api/order/{orderId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OrderDto>(json, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            try
            {
                await SetAuthorizationHeader();

                var response = await _httpClient.PostAsync($"/api/order/{orderId}/cancel", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error canceling order: {ex.Message}");
            }

            return false;
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
