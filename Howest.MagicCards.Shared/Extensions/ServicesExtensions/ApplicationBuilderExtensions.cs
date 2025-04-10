using Microsoft.AspNetCore.Builder;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseMagicCardsMiddlewares(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            return app;
        }
    }
}
