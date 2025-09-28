using Ecommerce.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // --- Seed Roles ---
            string adminRole = "Admin";
            string customerRole = "Customer";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            if (!await roleManager.RoleExistsAsync(customerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(customerRole));
            }

            // --- Seed Admin User ---
            string adminEmail = "admin@ecommerce.com";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true // Automatically confirm the email for the admin
                };

                // Create the user with a password
                IdentityResult result = await userManager.CreateAsync(adminUser, "Admin@123!");

                if (result.Succeeded)
                {
                    // Assign the "Admin" role to the new user
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}