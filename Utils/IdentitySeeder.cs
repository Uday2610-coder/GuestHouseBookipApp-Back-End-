using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using GuestHouseBookingApplication.Models.Entities;

namespace GuestHouseBookingApplication.Utils
{
    public static class IdentitySeeder
    {
        // THIS METHOD MUST BE PRESENT!
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var adminRole = "Admin";
            Console.WriteLine("Checking if Admin role exists...");

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                Console.WriteLine("Creating Admin role...");
                var role = new ApplicationRole { Name = adminRole, Description = "Administrator role" };
                await roleManager.CreateAsync(role);
            }

            Console.WriteLine("Checking if Admin user exists...");
            var adminEmail = "admin@guesthouse.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    Console.WriteLine("Assigning Admin role to user...");
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
                else
                {
                    throw new Exception("Failed to create admin: " + string.Join("; ", result.Errors));
                }
            }
        }
        public static async Task SeedRegularUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var userRole = "User";
            Console.WriteLine("Checking if User role exists...");

            if (!await roleManager.RoleExistsAsync(userRole))
            {
                Console.WriteLine("Creating User role...");
                var role = new ApplicationRole { Name = userRole, Description = "Regular user role" };
                await roleManager.CreateAsync(role);
            }

            Console.WriteLine("Checking if Regular user exists...");
            var userEmail = "user@guesthouse.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);

            if (regularUser == null)
            {
                regularUser = new ApplicationUser
                {
                    UserName = "user",
                    Email = userEmail,
                    FullName = "Regular User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(regularUser, "User@123");
                if (result.Succeeded)
                {
                    Console.WriteLine("Assigning User role to regular user...");
                    await userManager.AddToRoleAsync(regularUser, userRole);
                }
                else
                {
                    throw new Exception("Failed to create regular user: " + string.Join("; ", result.Errors));
                }
            }
        }
    }
}