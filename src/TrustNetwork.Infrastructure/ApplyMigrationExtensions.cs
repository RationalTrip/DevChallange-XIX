using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrustNetwork.Infrastructure.Context;

namespace TrustNetwork.Infrastructure
{
    public static class ApplyMigrationExtensions
    {
        public static async Task ApplyDbMigration(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TrustNetworkDbContext>();

            try
            {
                context.Database.SetCommandTimeout(300);
                await context.Database.MigrateAsync();
            }
            catch (SqlException ex)
            {
                var logger = scope.ServiceProvider.GetService<ILogger<TrustNetworkDbContext>>();

                if (logger == null)
                    throw;

                logger.LogError($"Migration error: {ex.Message}");
            }
        }
    }
}
