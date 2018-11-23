using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD.Data.Models.Identity;
using System;
using System.Threading.Tasks;

namespace SD.Web.Configurations
{
    public static class IdentityDataInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("User"))
            {
                IdentityRole newRole = new IdentityRole() { Name = "User" };
                await roleManager.CreateAsync(newRole);
            }

            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                IdentityRole newRole = new IdentityRole() { Name = "Administrator" };
                await roleManager.CreateAsync(newRole);
            }
        }

        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!await userManager.Users.AnyAsync(u => u.UserName == "Administrator"))
            {
				ApplicationUser newUser = new ApplicationUser()
				{
					UserName = Environment.GetEnvironmentVariable("SD_SuperAdminUserNameCredentials"),
					Email = Environment.GetEnvironmentVariable("SD_SuperAdminEmailCredentials"),
					CreatedOn = DateTime.UtcNow.AddHours(2),
					IsDeleted = false,
					IsAdmin = true
                };

                if ((await userManager.CreateAsync(newUser, Environment.GetEnvironmentVariable("SD_SuperAdminPasswordCredentials"))).Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Administrator");
                }
            }
        }
    }
}
