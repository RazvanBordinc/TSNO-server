using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TSNO.Services.Expiration;

namespace TSNO.BackgroundServices
{
    public class ExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExpirationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run the service indefinitely until stopped
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var expirationService = scope.ServiceProvider.GetRequiredService<IExpirationService>();

                    // Delete expired entities
                    await expirationService.DeleteExpiredEntitiesAsync();
                }

                // Wait for 5 minutes before running the check again
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
