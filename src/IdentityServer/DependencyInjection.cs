using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;
using LaserPointer.IdentityServer.Common.Interfaces;
using LaserPointer.IdentityServer.Common.Models;
using LaserPointer.IdentityServer.Common.Services;
using LaserPointer.IdentityServer.Infrastructure.Identity;
using LaserPointer.IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IDateTime = LaserPointer.IdentityServer.Common.Interfaces.IDateTime;

namespace LaserPointer.IdentityServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var globalSettings = new GlobalSettings();
            configuration.GetSection("GlobalSettings").Bind(globalSettings);
            services.AddSingleton(s => globalSettings);

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            
            return services;
        }
        
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                Console.WriteLine("Using in memory DB");
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("IdentityServerDb"));
            }
            else
            {
                Console.WriteLine("Using DefaultConnection string");
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }
        
        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();
            
            /* services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(); */
            
            var clients = configuration.GetSection("IdentityServer:Oid-Clients");
            // Console.WriteLine(configuration.GetSection("IdentityServer:Oid-Clients:0:ClientId").Value);
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
                .AddInMemoryClients(clients);
            
            services.AddTransient<IIdentityService, IdentityService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
