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

            // --- Seed Admin Users ---
            
            // First Admin Account
            string adminEmail = "admin@ecommerce.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(adminUser, "Admin@123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            // Second Admin Account
            string admin2Email = "mazen@ecommerce.com";
            if (await userManager.FindByEmailAsync(admin2Email) == null)
            {
                var admin2User = new ApplicationUser
                {
                    UserName = admin2Email,
                    Email = admin2Email,
                    FirstName = "Mazen",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin2User, "Mazen@123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin2User, adminRole);
                }
            }
        }
    }
}