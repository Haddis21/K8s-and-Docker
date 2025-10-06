using Order_Services.Models;

namespace Order_Services.Service
{
    public interface IRestaurantServiceClient
    {
        Task<RestaurantDto?> GetRestaurantAsync(int restaurantId);
        Task<MenuItemDto?> GetMenuItemAsync(int restaurantId, int menuItemId);
        Task<List<MenuItemDto>> GetMenuItemsAsync(int restaurantId);
    }
}
