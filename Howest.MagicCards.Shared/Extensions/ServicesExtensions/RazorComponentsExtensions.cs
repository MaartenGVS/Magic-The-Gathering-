using Microsoft.AspNetCore.Builder;

namespace Howest.MagicCards.Shared.Extensions
{
    public static class RazorComponentsExtensions
    {
        public static IApplicationBuilder UseRazorComponentMiddlewares(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();
            return app;
        }
    }
}
