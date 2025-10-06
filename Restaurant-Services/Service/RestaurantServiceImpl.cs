using Microsoft.EntityFrameworkCore;
using Restaurant_Services.Models;

namespace Restaurant_Services.Service
{
    public class RestaurantServiceImpl : IRestaurantService
    {
        private readonly AppDbContext _context;

        public RestaurantServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
        {
            var restaurants = await _context.Restaurants
                .Where(r => r.IsActive)
                .Include(r => r.MenuItems)
                .ToListAsync();

            return restaurants.Select(MapToDto);
        }

        public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.MenuItems)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            return restaurant != null ? MapToDto(restaurant) : null;
        }

        public async Task<RestaurantDto> CreateRestaurantAsync(CreateRestaurantDto createDto)
        {
            var restaurant = new Restaurant
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Address = new Address
                {
                    Street = createDto.Address.Street,
                    City = createDto.Address.City,
                    PostalCode = createDto.Address.PostalCode,
                    Latitude = createDto.Address.Latitude,
                    Longitude = createDto.Address.Longitude
                },
                CuisineType = createDto.CuisineType,
                DeliveryFee = createDto.DeliveryFee,
                EstimatedDeliveryTime = createDto.EstimatedDeliveryTime,
                ImageUrl = createDto.ImageUrl
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return MapToDto(restaurant);
        }

        public async Task<RestaurantDto?> UpdateRestaurantAsync(int id, CreateRestaurantDto updateDto)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return null;

            restaurant.Name = updateDto.Name;
            restaurant.Description = updateDto.Description;
            restaurant.Address.Street = updateDto.Address.Street;
            restaurant.Address.City = updateDto.Address.City;
            restaurant.Address.PostalCode = updateDto.Address.PostalCode;
            restaurant.Address.Latitude = updateDto.Address.Latitude;
            restaurant.Address.Longitude = updateDto.Address.Longitude;
            restaurant.CuisineType = updateDto.CuisineType;
            restaurant.DeliveryFee = updateDto.DeliveryFee;
            restaurant.EstimatedDeliveryTime = updateDto.EstimatedDeliveryTime;
            restaurant.ImageUrl = updateDto.ImageUrl;

            await _context.SaveChangesAsync();
            return MapToDto(restaurant);
        }

        public async Task<bool> DeleteRestaurantAsync(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return false;

            restaurant.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RestaurantDto>> SearchRestaurantsAsync(string query)
        {
            var restaurants = await _context.Restaurants
                .Where(r => r.IsActive &&
                           (r.Name.Contains(query) ||
                            r.CuisineType.Contains(query) ||
                            r.Description.Contains(query)))
                .Include(r => r.MenuItems)
                .ToListAsync();

            return restaurants.Select(MapToDto);
        }

        public async Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync(int restaurantId)
        {
            var menuItems = await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .ToListAsync();

            return menuItems.Select(MapMenuItemToDto);
        }

        public async Task<MenuItemDto> AddMenuItemAsync(int restaurantId, CreateMenuItemDto createDto)
        {
            var menuItem = new MenuItem
            {
                RestaurantId = restaurantId,
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Category = createDto.Category,
                IsAvailable = createDto.IsAvailable,
                ImageUrl = createDto.ImageUrl
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            return MapMenuItemToDto(menuItem);
        }

        public async Task<MenuItemDto?> UpdateMenuItemAsync(int restaurantId, int menuItemId, CreateMenuItemDto updateDto)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.RestaurantId == restaurantId);

            if (menuItem == null) return null;

            menuItem.Name = updateDto.Name;
            menuItem.Description = updateDto.Description;
            menuItem.Price = updateDto.Price;
            menuItem.Category = updateDto.Category;
            menuItem.IsAvailable = updateDto.IsAvailable;
            menuItem.ImageUrl = updateDto.ImageUrl;

            await _context.SaveChangesAsync();
            return MapMenuItemToDto(menuItem);
        }

        public async Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.RestaurantId == restaurantId);

            if (menuItem == null) return false;

            menuItem.IsAvailable = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        private static RestaurantDto MapToDto(Restaurant restaurant)
        {
            return new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                Address = new AddressDto
                {
                    Street = restaurant.Address.Street,
                    City = restaurant.Address.City,
                    PostalCode = restaurant.Address.PostalCode,
                    Latitude = restaurant.Address.Latitude,
                    Longitude = restaurant.Address.Longitude
                },
                CuisineType = restaurant.CuisineType,
                DeliveryFee = restaurant.DeliveryFee,
                EstimatedDeliveryTime = restaurant.EstimatedDeliveryTime,
                ImageUrl = restaurant.ImageUrl,
                IsActive = restaurant.IsActive,
                MenuItems = restaurant.MenuItems?.Select(MapMenuItemToDto).ToList() ?? new List<MenuItemDto>()
            };
        }

        private static MenuItemDto MapMenuItemToDto(MenuItem menuItem)
        {
            return new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                Price = menuItem.Price,
                Category = menuItem.Category,
                IsAvailable = menuItem.IsAvailable,
                ImageUrl = menuItem.ImageUrl
            };
        }
    }
}
