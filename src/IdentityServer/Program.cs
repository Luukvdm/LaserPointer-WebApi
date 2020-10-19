using System;
using System.Threading.Tasks;
using LaserPointer.IdentityServer.Infrastructure.Identity;
using LaserPointer.IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LaserPointer.IdentityServer
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlite())
                    {
                        System.IO.Directory.CreateDirectory("./sqlite");
                        await context.Database.MigrateAsync();
                    }

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
