using FoodOrderApp.Models;

namespace FoodOrderApp.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantDto>> GetRestaurantsAsync();
        Task<RestaurantDto?> GetRestaurantAsync(int id);
        Task<List<RestaurantDto>> SearchRestaurantsAsync(string query);
        Task<List<MenuItemDto>> GetMenuItemsAsync(int restaurantId);
    }
}
