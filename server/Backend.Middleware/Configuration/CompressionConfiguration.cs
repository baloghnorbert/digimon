using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;
using System.Linq;

namespace Backend.Middleware.Configuration
{
    public static class CompressionConfiguration
    {
        public static void AddApplicationCompression(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                /*
                 * use if custom compression are needed *
                 options.Providers.Add<CustomCompressionProvider>();
                */
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat( new[]
                {
                    "image/svg+xml"
                });
            }); ;

            serviceCollection.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        public static void AddApplicationCompression(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseResponseCompression();
        }
    }
}
