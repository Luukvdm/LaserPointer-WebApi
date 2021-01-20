using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using LaserPointer.WebApi.Application;
using LaserPointer.WebApi.Application.Common.Interfaces;
using LaserPointer.WebApi.Application.Common.Models;
using LaserPointer.WebApi.Infrastructure;
using LaserPointer.WebApi.Infrastructure.Persistence;
using LaserPointer.WebApi.WebApi.BackgroundServices;
using LaserPointer.WebApi.WebApi.Filters;
using LaserPointer.WebApi.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

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

            // Logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
            
			services.Configure<ForwardedHeadersOptions>(options =>
			{
				options.ForwardedHeaders =
					ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
			});

            services.AddApplication();
			services.AddInfrastructure(Configuration, Environment, globalSettings);
			services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IClientEventService, ClientEventService>();
			services.AddHttpContextAccessor();
			services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

			services.AddControllers(options =>
			{
				options.Filters.Add(new ApiExceptionFilterAttribute());
            }).AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
			}).AddFluentValidation();

			// Customise default API behaviour
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

            services.AddCors();
            
            // Temp Hash cracking service TODO: Remove in the future
            services.AddSingleton<IHostedService, HashCrackerService>();
            
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        /* ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{globalSettings.IdentityAuthority}/connect/authorize"),
                            TokenUrl = new Uri($"{globalSettings.IdentityAuthority}/connect/token")
                        }, */
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{globalSettings.IdentityAuthority}/connect/authorize"),
                            TokenUrl = new Uri($"{globalSettings.IdentityAuthority}/connect/token"),
                            Scopes = globalSettings.IdentityScopes
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        globalSettings.IdentityRequiredPolicies
                    }
                });
            });
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GlobalSettings globalSettings)
		{
			app.UsePathBase(globalSettings.BasePath);
            IdentityModelEventSource.ShowPII = true; 
            
            if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
            
                app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials
            }
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
                app.UseHttpsRedirection();
            }
            
            // Logging
            app.UseSerilogRequestLogging();

            app.UseForwardedHeaders();
            app.UseHealthChecks("/health");

			/* app.UseCors(policy => policy.WithOrigins(globalSettings.BaseServiceUri.ClientApp)
				.AllowAnyMethod().AllowAnyHeader().AllowCredentials()); */


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LaserPointer");
                c.OAuthClientSecret(globalSettings.IdentitySecret); // globalSettings.IdentitySecret
                c.OAuthClientId("");
                c.OAuthAppName("LaserPointer Swagger UI");
                c.OAuthScopeSeparator(" ");
                c.OAuthUsePkce();
            });
            
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => { 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                ); 
            });

        }
	}
}
