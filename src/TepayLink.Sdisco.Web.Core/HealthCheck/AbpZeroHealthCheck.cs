using Microsoft.Extensions.DependencyInjection;
using TepayLink.Sdisco.HealthChecks;

namespace TepayLink.Sdisco.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<SdiscoDbContextHealthCheck>("Database Connection");
            builder.AddCheck<SdiscoDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
