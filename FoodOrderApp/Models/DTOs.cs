using System.ComponentModel.DataAnnotations;

namespace FoodOrderApp.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public AddressDto? Address { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }

    // Restaurant DTOs
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
        public List<MenuItemDto> MenuItems { get; set; } = new();
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

    // Order DTOs
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }  // ADD THIS LINE
        public decimal ItemTotal => Price * Quantity;
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto DeliveryAddress { get; set; } = new();
        public string? SpecialInstructions { get; set; }  // ADD THIS LINE
        public DateTime CreatedAt { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
    }

    public class CreateOrderDto
    {
        public int RestaurantId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
        public AddressDto DeliveryAddress { get; set; } = new();
        public string? SpecialInstructions { get; set; }
    }

    public class CreateOrderItemDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
    }

    public class AddressDto
    {
        [Required]
        public string Street { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        Ready = 3,
        OutForDelivery = 4,
        Delivered = 5,
        Cancelled = 6
    }

    // Cart Models
    public class CartItem
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal ItemTotal => Price * Quantity;
    }

    public class Cart
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.ItemTotal);
        public int ItemCount => Items.Sum(i => i.Quantity);
    }
}
