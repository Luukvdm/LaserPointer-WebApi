using System.Linq;
using System.Threading.Tasks;
using LaserPointer.IdentityServer.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace LaserPointer.IdentityServer.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultAdmin = new ApplicationUser { UserName = "admin@laserpointer.com", Email = "admin@laserpointer.com" };

            if (userManager.Users.All(u => u.UserName != defaultAdmin.UserName))
            {
                await userManager.CreateAsync(defaultAdmin, "@Welkom1");
            }
            
            var defaultUser = new ApplicationUser { UserName = "user@laserpointer.com", Email = "user@laserpointer.com" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "@Welkom1");
            }
        }
    }
}
