using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Backend.Middleware.HealthStatus;

namespace Backend.Middleware.Configuration
{
    public static class HealthChecksConfiguration
    {
        public static void AddAppHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddCheck<HealthCheckAPI>("HealthCheckAPI");
        }

        public static void AddAppHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/health", new HealthCheckOptions()
            {
                ResponseWriter = (httpContext, result) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    JObject json = new JObject(
                        new JProperty("status", result.Status.ToString()),
                        new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString())))))));
                    
                    return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
                }
            });
        }
    }
}
