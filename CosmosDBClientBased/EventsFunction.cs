using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CosmosDBClientBased.Models;
using CosmosDBClientBased;

namespace AttributeBased;

public class EventsFunction
{
    private readonly ILogger<EventsFunction> _logger;
    private readonly FirstAidEventService _firstAidEventService;

    public EventsFunction(ILogger<EventsFunction> logger, FirstAidEventService firstAidEventService)
    {
        _logger = logger;
        _firstAidEventService = firstAidEventService;
    }

    [Function("Events")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Events/{id?}")] HttpRequest req, Guid? id)
    {
        if (id is not null)
        {
            var eventById = await _firstAidEventService.GetEventByIdAsync(id.Value);
            return new OkObjectResult(eventById);
        }

        var events = await _firstAidEventService.GetEventsAsync();
        return new OkObjectResult(events);
    }

    [Function("AddEvent")]
    public async Task<IActionResult> AddEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        var newEvent = await req.ReadFromJsonAsync<FirstAidEvent>();
        if (newEvent == null)
        {
            return new BadRequestObjectResult("Invalid event data.");
        }

        await _firstAidEventService.AddEventAsync(newEvent);
        return new OkObjectResult(newEvent);
    }

    [Function("UpdateEvent")]
    public async Task<IActionResult> UpdateEvent([HttpTrigger(AuthorizationLevel.Anonymous, "put")] HttpRequest req)
    {
        var updatedEvent = await req.ReadFromJsonAsync<FirstAidEvent>();
        if (updatedEvent == null)
        {
            return new BadRequestObjectResult("Invalid event data.");
        }

        await _firstAidEventService.UpdateEventAsync(updatedEvent);
        return new OkObjectResult(updatedEvent);
    }

    [Function("SoftDeleteEvent")]
    public async Task<IActionResult> SoftDeleteEvent([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequest req)
    {
        var eventId = req.Query["id"];
        if (string.IsNullOrEmpty(eventId))
        {
            return new BadRequestObjectResult("Invalid event ID.");
        }

        await _firstAidEventService.SoftDeleteEventAsync(Guid.Parse(eventId));
        return new OkObjectResult($"Event with ID {eventId} has been soft deleted.");
    }


}