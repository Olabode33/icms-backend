using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ICMSDemo.EntityFrameworkCore;

namespace ICMSDemo.HealthChecks
{
    public class ICMSDemoDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public ICMSDemoDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("ICMSDemoDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("ICMSDemoDbContext could not connect to database"));
        }
    }
}
