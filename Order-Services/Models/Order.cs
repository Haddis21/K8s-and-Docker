using System.ComponentModel.DataAnnotations;

namespace Order_Services.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        [Range(0, 10000)]
        public decimal Subtotal { get; set; }

        [Range(0, 200)]
        public decimal DeliveryFee { get; set; }

        [Range(0, 1000)]
        public decimal Tax { get; set; }

        [Range(0, 10000)]
        public decimal Total { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        public Address DeliveryAddress { get; set; } = new();

        public string? SpecialInstructions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuItemName { get; set; } = string.Empty;

        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Range(1, 50)]
        public int Quantity { get; set; }

        public string? SpecialInstructions { get; set; }

        public Order Order { get; set; } = null!;
    }

    public class Address
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
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
}
