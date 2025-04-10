using Howest.MagicCards.DAL.Repositories;
using StackExchange.Redis;

namespace Howest.MagicCards.MinimalAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddSingleton<IConnectionMultiplexer>(opt =>
                ConnectionMultiplexer.Connect(ConfigurationExtensions.GetConnectionString(configuration, "Redis"))
            );
            serviceCollection.AddScoped<IDeckRepository, RedisDeckRepository>();
        }
    }
}
