using Restaurant_Services.Models;

namespace Restaurant_Services.Service
{
    public interface IRestaurantService
    {
        
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        Task<RestaurantDto?> GetRestaurantByIdAsync(int id);
        Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantDto createDto);
        Task<RestaurantDto?> UpdateRestaurantAsync(int id, CreateRestaurantDto updateDto);
        Task<bool> DeleteRestaurantAsync(int id);
        Task<IEnumerable<RestaurantDto>> SearchRestaurantsAsync(string query);
        Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync(int restaurantId);
        Task<MenuItemDto> AddMenuItemAsync(int restaurantId, CreateMenuItemDto createDto);
        Task<MenuItemDto?>  UpdateMenuItemAsync(int restaurantId, int menuItemId, CreateMenuItemDto updateDto);
        Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId);
    
}
}
