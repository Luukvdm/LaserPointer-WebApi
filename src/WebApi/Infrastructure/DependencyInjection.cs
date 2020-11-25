using System;
using System.Security.Claims;
using IdentityModel;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using LaserPointer.WebApi.Application.Common.Behaviours;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Infrastructure.Persistence;
using LaserPointer.WebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace LaserPointer.WebApi.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration,
			IWebHostEnvironment environment,
			GlobalSettings globalSettings)
		{
			if (configuration.GetValue<bool>("UseInMemoryDatabase"))
			{
                Console.WriteLine("Using in memory DB");
				services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("SchemaDb"));
			}
			else
			{
				Console.WriteLine("Using DefaultConnection string");
				services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
						b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
			}

			services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            
            services.AddScoped<IDomainEventService, DomainEventService>();

			services.AddTransient<IDateTime, DateTimeService>();

            services
				.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = globalSettings.IdentityAuthority;
					options.RequireHttpsMetadata = !environment.IsDevelopment() &&
					                               globalSettings.IdentityAuthority.StartsWith("https");
					options.ApiSecret = globalSettings.IdentitySecret;
					options.TokenRetriever = TokenRetrieval.FromAuthorizationHeader();
					options.NameClaimType = ClaimTypes.Email;
					options.SupportedTokens = SupportedTokens.Jwt;
				});

			services.AddAuthorization(config =>
			{
				config.AddPolicy("Application", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(JwtClaimTypes.Scope, "WebApi");
				});
			});

			if (environment.IsDevelopment())
			{
				IdentityModelEventSource.ShowPII = true;
			}

			return services;
		}
	}
}
