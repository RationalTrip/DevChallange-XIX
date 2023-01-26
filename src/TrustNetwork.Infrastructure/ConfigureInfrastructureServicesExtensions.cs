using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Application.Services;
using TrustNetwork.Infrastructure.Context;
using TrustNetwork.Infrastructure.Repositories;
using TrustNetwork.Infrastructure.Services;

namespace TrustNetwork.Infrastructure
{
    public static class ConfigureInfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            services.AddTrustNetworkDbContext(configuration, env);

            services.AddRepositories();

            services.AddServices();

            return services;
        }

        private static IServiceCollection AddTrustNetworkDbContext(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            if (env.IsProduction())
            {
                string connectionString = configuration.GetConnectionString("TrustNetworkDb");
                services.AddDbContext<TrustNetworkDbContext>(conf =>
                {
                    conf.UseSqlServer(connectionString);
                });
            }
            else
            {
                services.AddDbContext<TrustNetworkDbContext>(conf =>
                {
                    conf.UseInMemoryDatabase("TrustNetworkDb");
                });
            }

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<ITopicsRepository, TopicsRepository>();
            services.AddScoped<IRelationsRepository, RelationRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<IMessageService, MessageService>();

            return services;
        }
    }
}
