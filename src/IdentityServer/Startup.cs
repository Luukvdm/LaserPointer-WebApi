using LaserPointer.IdentityServer.Common.Models;
using LaserPointer.IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable MemberCanBePrivate.Global

namespace LaserPointer.IdentityServer
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders =
					ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

            services.AddBaseServices(Configuration);
            services.AddPersistence(Configuration);
            services.AddIdentityServer(Configuration);

            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
            services.AddRazorPages();

			// Customise default API behaviour
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GlobalSettings globalSettings)
		{
			app.UsePathBase(globalSettings.BasePath);
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
				app.UseForwardedHeaders();
			}
			else
			{
				app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHealthChecks("/health");
			app.UseStaticFiles();

            app.UseCors(policy => policy.SetIsOriginAllowed(h => true)
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials());

			app.UseRouting();

			app.UseAuthentication();
            app.UseAuthorization();
			app.UseIdentityServer();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
    }
}
