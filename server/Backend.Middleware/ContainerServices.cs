using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Backend.Middleware
{
    public static class ContainerServices
    {
        public static void InjectContainerServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // Add useful interface for accessing the ActionContext outside a controller.
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Add useful interface for accessing the HttpContext outside a controller.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add useful interface for accessing the IUrlHelper outside a controller.
            services.AddScoped<IUrlHelper>(x => x.GetRequiredService<IUrlHelperFactory>()
                                                 .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
        }
    }
}
