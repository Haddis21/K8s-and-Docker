using Microsoft.AspNetCore.Identity;

namespace User_Services.Models
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            // Create roles
            string[] roles = { "Admin", "Customer", "RestaurantOwner" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }

            // Create admin user
            if (await userManager.FindByEmailAsync("admin@foodordering.com") == null)
            {
                var adminUser = new User
                {
                    UserName = "admin@foodordering.com",
                    Email = "admin@foodordering.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    PhoneNumber = "+46701234567",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Create test customer
            if (await userManager.FindByEmailAsync("customer@test.com") == null)
            {
                var customerUser = new User
                {
                    UserName = "customer@test.com",
                    Email = "customer@test.com",
                    FirstName = "Test",
                    LastName = "Customer",
                    PhoneNumber = "+46701234568",
                    EmailConfirmed = true,
                    Address = new Address
                    {
                        Street = "Testgatan 1",
                        City = "Stockholm",
                        PostalCode = "11122",
                        Latitude = 59.3293,
                        Longitude = 18.0686
                    }
                };

                var result = await userManager.CreateAsync(customerUser, "Customer123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }
        }
    }
}
