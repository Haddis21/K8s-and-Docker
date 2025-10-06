namespace Restaurant_Services.Models
{
    public class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            var restaurants = new[]
            {
                new Restaurant
                {
                    Name = "Pizza Palace",
                    Description = "Authentic Italian pizza with fresh ingredients",
                    Address = new Address
                    {
                        Street = "123 Main St",
                        City = "Stockholm",
                        PostalCode = "11122",
                        Latitude = 59.3293,
                        Longitude = 18.0686
                    },
                    CuisineType = "Italian",
                    DeliveryFee = 29.00m,
                    EstimatedDeliveryTime = 35,
                    ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?w=400",
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem { Name = "Margherita Pizza", Description = "Classic tomato, mozzarella, basil", Price = 149.00m, Category = "Pizza", ImageUrl = "https://images.unsplash.com/photo-1604382354936-07c5d9983bd3?w=300" },
                        new MenuItem { Name = "Pepperoni Pizza", Description = "Tomato sauce, mozzarella, pepperoni", Price = 169.00m, Category = "Pizza", ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=300" },
                        new MenuItem { Name = "Caesar Salad", Description = "Romaine lettuce, parmesan, croutons", Price = 89.00m, Category = "Salads", ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=300" }
                    }
                },
                new Restaurant
                {
                    Name = "Burger Barn",
                    Description = "Juicy burgers made with premium beef",
                    Address = new Address
                    {
                        Street = "456 Oak Ave",
                        City = "Stockholm",
                        PostalCode = "11123",
                        Latitude = 59.3345,
                        Longitude = 18.0632
                    },
                    CuisineType = "American",
                    DeliveryFee = 39.00m,
                    EstimatedDeliveryTime = 25,
                    ImageUrl = "https://images.unsplash.com/photo-1571091718767-18b5b1457add?w=400",
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem { Name = "Classic Burger", Description = "Beef patty, lettuce, tomato, onion", Price = 129.00m, Category = "Burgers", ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=300" },
                        new MenuItem { Name = "Cheese Burger", Description = "Classic burger with cheddar cheese", Price = 139.00m, Category = "Burgers", ImageUrl = "https://images.unsplash.com/photo-1586190848861-99aa4a171e90?w=300" },
                        new MenuItem { Name = "French Fries", Description = "Crispy golden fries", Price = 49.00m, Category = "Sides", ImageUrl = "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?w=300" }
                    }
                },
                new Restaurant
                {
                    Name = "Sushi Zen",
                    Description = "Fresh sushi and Japanese cuisine",
                    Address = new Address
                    {
                        Street = "789 Sakura St",
                        City = "Stockholm",
                        PostalCode = "11124",
                        Latitude = 59.3251,
                        Longitude = 18.0710
                    },
                    CuisineType = "Japanese",
                    DeliveryFee = 45.00m,
                    EstimatedDeliveryTime = 40,
                    ImageUrl = "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?w=400",
                    MenuItems = new List<MenuItem>
                    {
                        new MenuItem { Name = "Salmon Sashimi", Description = "Fresh Norwegian salmon", Price = 189.00m, Category = "Sashimi", ImageUrl = "https://images.unsplash.com/photo-1579952363873-27d3bfad9c0d?w=300" },
                        new MenuItem { Name = "California Roll", Description = "Crab, avocado, cucumber", Price = 149.00m, Category = "Rolls", ImageUrl = "https://images.unsplash.com/photo-1579952363873-27d3bfad9c0d?w=300" },
                        new MenuItem { Name = "Miso Soup", Description = "Traditional soybean soup", Price = 39.00m, Category = "Soup", ImageUrl = "https://images.unsplash.com/photo-1604909052743-94e838986d24?w=300" }
                    }
                }
            };

            context.Restaurants.AddRange(restaurants);
            context.SaveChanges();
        }
    
}
}
