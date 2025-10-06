using System.ComponentModel.DataAnnotations;

namespace Order_Services.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto DeliveryAddress { get; set; } = new();
        public string? SpecialInstructions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public decimal ItemTotal => Price * Quantity;
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateOrderItemDto> Items { get; set; } = new();

        [Required]
        public AddressDto DeliveryAddress { get; set; } = new();

        public string? SpecialInstructions { get; set; }
    }

    public class CreateOrderItemDto
    {
        [Required]
        public int MenuItemId { get; set; }

        [Range(1, 50)]
        public int Quantity { get; set; }

        public string? SpecialInstructions { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }
    }

    // External service DTOs (from other microservices)
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public int EstimatedDeliveryTime { get; set; }
        public List<MenuItemDto> MenuItems { get; set; } = new();
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
