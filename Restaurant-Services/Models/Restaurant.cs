using System.ComponentModel.DataAnnotations;

namespace Restaurant_Services.Models
{
   
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Address Address { get; set; } = new();

        [StringLength(50)]
        public string CuisineType { get; set; } = string.Empty;

        [Range(0, 50)]
        public decimal DeliveryFee { get; set; }

        [Range(10, 120)]
        public int EstimatedDeliveryTime { get; set; } // minutes

        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<MenuItem> MenuItems { get; set; } = new();
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

    public class MenuItem
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Restaurant Restaurant { get; set; } = null!;
    }
}
