using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using AttributeBased.Models;
using Microsoft.Azure.Functions.Worker.Extensions;

namespace AttributeBased;

public class EventsFunction
{
    private readonly ILogger<EventsFunction> _logger;

    public EventsFunction(ILogger<EventsFunction> logger)
    {
        _logger = logger;
    }

    [Function("Events")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
        [CosmosDBInput(
            databaseName: "eventplanning",
            containerName: "events",
            Connection = "CosmosDBConnection",
            SqlQuery = "SELECT * FROM c where c.IsDeleted = false"
        )] IEnumerable<FirstAidEvent> events)

    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(events);
        
    }


    [Function("AddEvents")]
    [CosmosDBOutput(databaseName: "eventplanning",
            containerName: "events",
            Connection = "CosmosDBConnection", CreateIfNotExists = false)]
    public async Task<FirstAidEvent> AddEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
         HttpRequest req)
    {
        var newEvent = await req.ReadFromJsonAsync<FirstAidEvent>();

        newEvent.id = Guid.NewGuid();
               
        return newEvent;
    }

}