using Howest.MagicCards.DAL.DBContext;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Howest.MagicCards.Shared.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMagicCardsDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CardsContext>(options => options.UseSqlServer(ConfigurationExtensions.GetConnectionString(config, "CardsDb")));
            return services;
        }

        public static IServiceCollection AddMagicCardsRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICardRepository, SqlCardRepository>();
            services.AddScoped<IArtistRepository, SqlArtistRepository>();
            services.AddScoped<ITypeRepository, SqlTypeRepository>();
            services.AddScoped<ISetRepository, SqlSetRepository>();
            services.AddScoped<IColorRepository, SqlColorRepository>();
            return services;
        }

        public static IServiceCollection AddMagicCardsAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CardsProfile), typeof(ArtistsProfile), typeof(TypesProfile), typeof(SetsProfile), typeof(ColorsProfile));
            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration config)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = ConfigurationExtensions.GetConnectionString(config, "Redis");
                options.InstanceName = "MTG_Redis_";
            });
            return services;
        }
    }
}
