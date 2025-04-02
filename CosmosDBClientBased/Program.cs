using CosmosDBClientBased;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<FirstAidEventService>(_ =>
{
    var connectionString = string.IsNullOrEmpty(builder.Configuration["ConnectionStrings:CosmosDBConnection"]) ?
        builder.Configuration["ConnectionStrings_CosmosDBConnection"] : builder.Configuration["ConnectionStrings:CosmosDBConnection"];

    var cosmosClient = new CosmosClient(connectionString);
    return new FirstAidEventService(cosmosClient);
});


builder.Build().Run();