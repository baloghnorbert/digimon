using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Backend.Core.Constans;

namespace Backend.Middleware.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddApplicationSwagger(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc(ApplicationSettingsConstans.PublicAPI, new OpenApiInfo { Title = "http:\\tanulmanyiversenyek.hu", Version = ApplicationSettingsConstans.PublicAPI });

                // This code allow you to use XML-comments
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    x.IncludeXmlComments(xmlPath);
                }

                x.AddSecurityDefinition
                (
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token into the field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    }
                );
            });
        }

        public static void AddApplicationSwagger(this IApplicationBuilder applicationBuilder)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            applicationBuilder.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            applicationBuilder.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint($"/swagger/{ApplicationSettingsConstans.PublicAPI}/swagger.json", "tanulmanyiversenyek.hu public API");
            });
        }
    }
}
