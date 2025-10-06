using System.ComponentModel.DataAnnotations;

namespace Restaurant_Services.Models
{

    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new();
        public string CuisineType { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public int EstimatedDeliveryTime { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<MenuItemDto> MenuItems { get; set; } = new();
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class CreateRestaurantDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public AddressDto Address { get; set; } = new();
        public string CuisineType { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public int EstimatedDeliveryTime { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class CreateMenuItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
