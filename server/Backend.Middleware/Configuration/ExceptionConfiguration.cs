using Microsoft.AspNetCore.Builder;
using tanulmanyiversenyek.WebAPI.Middleware;

namespace Backend.Middleware.Configuration
{
    public static class ExceptionConfiguration
    {
        public static IApplicationBuilder AddApplicationExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
