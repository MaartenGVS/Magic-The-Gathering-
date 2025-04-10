using Howest.MagicCards.Shared.Extensions;
using Howest.MagicCards.Web.Components;
using Howest.MagicCards.Web.Services;
using MatBlazor;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();
ConfigureMiddleware(app);
app.Run();



void ConfigureServices(IServiceCollection services)
{
    services.AddRazorComponents()
            .AddInteractiveServerComponents();

    services.AddHttpClient<ICardService, CardService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7195/api/v1.5/");
    });

    services.AddHttpClient<IDeckService, DeckService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7079/api/");
    });

    services.AddMatBlazor();
    services.AddMatToaster(config =>
    {
        config.Position = MatToastPosition.BottomRight;
        config.PreventDuplicates = true;
        config.NewestOnTop = true;
        config.ShowCloseButton = true;
        config.MaximumOpacity = 100;
        config.VisibleStateDuration = 6000;
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseRazorComponentMiddlewares();

    app.MapRazorComponents<App>()
       .AddInteractiveServerRenderMode();
}
