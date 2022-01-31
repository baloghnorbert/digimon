using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Middleware.HealthStatus
{
    public class HealthCheckAPI : IHealthCheck
    {
        private HttpClient _client { get; }
        private readonly ILogger _logger;

        public HealthCheckAPI(HttpClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpResponseMessage response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _client.BaseAddress));
                response.EnsureSuccessStatusCode();

                return HealthCheckResult.Healthy();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
