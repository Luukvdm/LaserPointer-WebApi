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
            var defaultUser = new ApplicationUser { UserName = "admin", Email = "admin@laserpointer.com" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "@Welkom1");
            }
        }
    }
}
