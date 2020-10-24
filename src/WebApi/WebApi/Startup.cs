using System.Text.Json;
using System.Text.Json.Serialization;
using LaserPointer.WebApi.Application;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Infrastructure;
using LaserPointer.WebApi.Infrastructure.Persistence;
using LaserPointer.WebApi.WebApi.Filters;
using LaserPointer.WebApi.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LaserPointer.WebApi.WebApi {
public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		public void ConfigureServices(IServiceCollection services)
		{
            var globalSettings = new GlobalSettings();
            Configuration.GetSection("GlobalSettings").Bind(globalSettings);
            services.AddSingleton(s => globalSettings);

			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders =
					ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

			services.AddApplication();
			services.AddInfrastructure(Configuration, Environment, globalSettings);
			services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IServerSentEventsService, ServerSentEventsService>();
			services.AddHttpContextAccessor();
			services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

			services.AddControllers(options =>
			{
				options.Filters.Add(new ApiExceptionFilterAttribute());
			}).AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
			});

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
				app.UseHsts();
			}

            app.UseHealthChecks("/health");
			app.UseRouting();

			app.UseCors(policy => policy.WithOrigins(globalSettings.BaseServiceUri.ClientApp)
				.AllowAnyMethod().AllowAnyHeader().AllowCredentials());

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}
