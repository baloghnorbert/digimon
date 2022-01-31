using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Middleware.Configuration
{
    public static class WebSocketsConfiguration
    {
        public static void AddApplicationWebSockets(this IServiceCollection serviceCollection)
        { }

        //If you would like to accept WebSocket requests in a controller, the call to app.UseWebSockets must occur before app.UseEndpoints.
        public static void AddApplicationWebSockets(this IApplicationBuilder applicationBuilder)
        {


            WebSocketOptions webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };

            // webSocketOptions.AllowedOrigins.Add("");

            applicationBuilder.UseWebSockets(webSocketOptions);
        }
    }
}
