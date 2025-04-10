using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.GraphQL;
using Howest.MagicCards.Shared.Extensions;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
ConfigureServices(builder.Services, configuration);
var app = builder.Build();
ConfigureApp(app);
app.Run();


void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddMagicCardsDatabase(configuration);

    services.AddScoped<ICardRepository, SqlCardRepository>();
    services.AddScoped<IArtistRepository, SqlArtistRepository>();

    services.AddScoped<RootSchema>();
    services.AddGraphQL(options => options.EnableMetrics = true)
            .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
            .AddDataLoader()
            .AddSystemTextJson()
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true);
}

void ConfigureApp(WebApplication app)
{
    app.UseGraphQL<RootSchema>();
    app.UseGraphQLPlayground(
        "/ui/playground",
        new PlaygroundOptions()
        {
            EditorTheme = EditorTheme.Light
        });
}
