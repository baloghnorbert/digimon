using Microsoft.AspNetCore.Builder;
using tanulmanyiversenyek.WebAPI.Middleware;

namespace tanulmanyiversenyek.WebAPI
{
    public static class OptionsVerbConfiguration
    {
        public static IApplicationBuilder AddApplicationOptionsVerbHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OptionsVerbMiddleware>();
        }
    }
}
