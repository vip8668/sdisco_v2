using System;
using System.Threading;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TepayLink.Sdisco.EntityFrameworkCore;

namespace TepayLink.Sdisco.HealthChecks
{
    public class SdiscoDbContextUsersHealthCheck : IHealthCheck
    {
        private readonly IDbContextProvider<SdiscoDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SdiscoDbContextUsersHealthCheck(
            IDbContextProvider<SdiscoDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    // Switching to host is necessary for single tenant mode.
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        if (!await _dbContextProvider.GetDbContext().Database.CanConnectAsync(cancellationToken))
                        {
                            return HealthCheckResult.Unhealthy(
                                "SdiscoDbContext could not connect to database"
                            );
                        }

                        var user = await _dbContextProvider.GetDbContext().Users.AnyAsync(cancellationToken);
                        uow.Complete();

                        if (user)
                        {
                            return HealthCheckResult.Healthy("SdiscoDbContext connected to database and checked whether user added");
                        }

                        return HealthCheckResult.Unhealthy("SdiscoDbContext connected to database but there is no user.");

                    }
                }
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy("SdiscoDbContext could not connect to database.", e);
            }
        }
    }
}
