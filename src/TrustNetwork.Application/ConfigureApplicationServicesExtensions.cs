using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TrustNetwork.Application.Dtos.Relation;
using TrustNetwork.Application.Profiles;

namespace TrustNetwork.Application
{
    public static class ConfigureApplicationServicesExtensions
    {
        public static JsonSerializerOptions AddJsonSerializers(this JsonSerializerOptions options)
        {
            options.Converters.Add(new RelationCreateDtoJsonConverter());

            return options;
        }

        public static IServiceCollection AddAplicationServices(this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment env)
        {
            services.AddMapper();

            return services;
        }

        private static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<PersonsProfile>();
            });

            return services;
        }
    }
}
