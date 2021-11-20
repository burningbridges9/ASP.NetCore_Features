using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseService.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthy = await IsDbConnectionOkAsync(); 
            return healthy
                ? HealthCheckResult.Healthy("Database connection is OK")
                : HealthCheckResult.Unhealthy("Database connection ERROR");
        }

        private Task<bool> IsDbConnectionOkAsync()
        {
            return Task.FromResult(DateTime.Now.Millisecond % 2 == 0);
        }
    }
}
