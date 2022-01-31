using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Backend.Middleware.Configuration
{
    public static class CorsConfiguration
    {
        public static void AddApplicationCors(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            string[] origins = BuildOrigins();

            services.AddCors(options =>
            {
#if DEBUG
                options.AddPolicy("Debug", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
#else

                options.AddPolicy("Production", builder =>
                {
                    builder.WithOrigins(origins)
                           .SetIsOriginAllowedToAllowWildcardSubdomains()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
#endif
            });
        }

        public static void AddApplicationCors(this IApplicationBuilder applicationBuilder)
        {
#if DEBUG
            applicationBuilder.UseCors("Debug");
#else
            applicationBuilder.UseCors("Production");
#endif
        }

        private static string[] BuildOrigins()
        {
            string portalOrigin = "mywebsite.com";
            string localhost3000 = "localhost:3000";

            List<string> origins = new List<string>()
            {
                $"https://{portalOrigin}", $"http://{portalOrigin}", $"https://*.{portalOrigin}", $"http://*.{portalOrigin}",
                $"https://{localhost3000}", $"http://{localhost3000}", $"https://*.{localhost3000}", $"http://*.{localhost3000}"
            };

            return origins.ToArray();
        }
    }
}
