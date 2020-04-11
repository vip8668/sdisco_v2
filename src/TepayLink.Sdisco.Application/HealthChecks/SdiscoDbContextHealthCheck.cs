using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TepayLink.Sdisco.EntityFrameworkCore;

namespace TepayLink.Sdisco.HealthChecks
{
    public class SdiscoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public SdiscoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("SdiscoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("SdiscoDbContext could not connect to database"));
        }
    }
}
