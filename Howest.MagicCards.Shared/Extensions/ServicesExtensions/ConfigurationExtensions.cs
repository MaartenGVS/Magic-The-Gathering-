using Microsoft.Extensions.Configuration;

namespace Howest.MagicCards.Shared.Extensions.ServicesExtensions
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfiguration config, string dbString)
        {
            return config[$"ConnectionStrings:{dbString}"];
        }

        public static string GetApiUrlPrefix(this IConfiguration config)
        {
            return config["ApiUrlPrefix"];
        }
    }
}
