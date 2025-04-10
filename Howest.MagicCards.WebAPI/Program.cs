using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

ConfigureServices(builder.Services, config);
var app = builder.Build();
ConfigureMiddleware(app);
app.Run();


void ConfigureServices(IServiceCollection services, ConfigurationManager config)
{
    services.AddControllers();
    services.AddRedisCache(config);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(ConfigureSwagger);
    services.AddApiVersioning(ConfigureApiVersioning);
    services.AddVersionedApiExplorer(ConfigureVersionedApiExplorer);
    services.AddMagicCardsDatabase(config);
    services.AddMagicCardsRepositories();
    services.AddMagicCardsAutoMapper();
}

void ConfigureSwagger(SwaggerGenOptions options)
{
    options.SwaggerDoc("v1.1", new OpenApiInfo
    {
        Title = "Howest MagicCards API v1.1",
        Version = "v1.1",
        Description = "This is the version 1.1 of the Magic Cards API"
    });
    options.SwaggerDoc("v1.5", new OpenApiInfo
    {
        Title = "Howest MagicCards API v1.5",
        Version = "v1.5",
        Description = "This is the version 1.5 of the Magic Cards API"
    });
}

void ConfigureApiVersioning(ApiVersioningOptions options)
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
}

void ConfigureVersionedApiExplorer(ApiExplorerOptions options)
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MTG v1.1");
        options.SwaggerEndpoint("/swagger/v1.5/swagger.json", "MTG v1.5");
    });

    app.UseMagicCardsMiddlewares();
    app.MapControllers();
}
