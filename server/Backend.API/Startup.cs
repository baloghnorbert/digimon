using Backend.Middleware;
using Backend.Middleware.Configuration;
using Backend.Middleware.Providers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using tanulmanyiversenyek.WebAPI;
using tanulmanyiversenyek.WebAPI.Filters;

namespace Backend.API
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            if (env.IsDevelopment())
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                                          .AddJsonFile($"appsettings.development.json", optional: true)
                                                                          .AddEnvironmentVariables();

                Configuration = builder.Build();
            }
            else
            {
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile($"/var/www/{{mywebsite}}/appsettings.production.json", optional: true)
                                                                          .AddEnvironmentVariables();

                Configuration = builder.Build();
            }

            string envPath = Path.Combine(env.ContentRootPath, ".env");
            DotNetEnv.Env.Load(envPath);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InjectContainerServices(Configuration);

            services
                .Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                })
                .AddMvcCore(options =>
                {
                    // Add custom binder provider for mapping json object form multipart/form-data
                    options.ModelBinderProviders.Insert(0, new JsonModelBinderProvider());
                    // Add "Cache-Control" header
                    options.Filters.Add(typeof(CacheControlFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
#if DEBUG
                    options.SerializerSettings.Formatting = Formatting.Indented;
#else
                    options.SerializerSettings.Formatting = Formatting.None;
#endif
                })
                .AddFluentValidation(options =>
                {
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.ImplicitlyValidateChildProperties = true;
                })
                .AddApiExplorer()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddApplicationVersioning();
            services.AddApplicationCookieAuthentication();
            services.AddApplicationCompression();
            services.AddApplicationCors();
            services.AddRazorPages();
            services.AddRouting();
            services.AddControllersWithViews();
            services.AddAppHealthChecks();
            services.AddHttpClient();
            services.AddApplicationSwagger();
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            // Use an exception handler middleware before any other handlers
            app.AddApplicationExceptionHandler();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            IServiceProvider serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            app.UseRouting();
            app.AddApplicationCompression();
            app.AddApplicationSwagger();
            app.UseAuthorization();
            app.AddApplicationCors();
            app.AddApplicationOptionsVerbHandler(); // Options verb handler must be added after CORS. See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#cross-origin-resource-sharing-cors-and-preflight-requests
            app.AddAppHealthChecks();


            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            logger.LogInformation("Web API up and runing.");
            logger.LogInformation("DB up and runing.");
            logger.LogInformation("Server configuration is completed");
        }
    }
}
