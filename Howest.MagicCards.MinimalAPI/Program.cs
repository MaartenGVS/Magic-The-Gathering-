using FluentValidation;
using FluentValidation.AspNetCore;
using Howest.MagicCards.MinimalAPI.Extensions;
using Howest.MagicCards.Shared.DTO.DeckDTO;
using Howest.MagicCards.Shared.Mappings;
using Howest.MagicCards.Shared.Validation;
using ConfigurationExtensions = Howest.MagicCards.Shared.Extensions.ServicesExtensions.ConfigurationExtensions;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
ConfigureServices(services, configuration);
var app = builder.Build();
ConfigureMiddleware(app, configuration);
app.Run();


void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddServices(configuration);
    services.AddAutoMapper(new Type[] { typeof(DeckCardsProfile) });

    services.AddFluentValidationAutoValidation();
    services.AddScoped<IValidator<DeckEntryWriteDTO>, DeckCardCustomValidation>();
}

void ConfigureMiddleware(WebApplication app, ConfigurationManager configuration)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    string urlPrefix = ConfigurationExtensions.GetApiUrlPrefix(configuration);

    var deckCardsGroup = app.MapGroup($"{urlPrefix}/deckEntries")
                            .WithTags("DeckEntries");

    deckCardsGroup.MapDeckCardEndpoints(urlPrefix);
}
