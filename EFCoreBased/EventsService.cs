using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFCoreBased;
using Microsoft.EntityFrameworkCore;
using EFCoreBased.Models;

namespace AttributeBased;

public class EventsService
{
    private readonly ILogger<EventsService> _logger;
    private readonly EventContext eventContext;

    public EventsService(ILogger<EventsService> logger, EventContext eventContext)
    {
        _logger = logger;
        this.eventContext = eventContext;
    }

    [Function("Event")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var events = await eventContext.Events.ToListAsync();
        return new OkObjectResult(events);
        
    }

    [Function("AddEvent")]
    public async Task<IActionResult> AddEvent([HttpTrigger(AuthorizationLevel.Anonymous, "post")]
         HttpRequest req)
    {
        var newEvent = await req.ReadFromJsonAsync<FirstAidEvent>();
        if (newEvent == null)
        {
            return new BadRequestObjectResult("Invalid event data.");
        }

        await eventContext.Events.AddAsync(newEvent);
        await eventContext.SaveChangesAsync();

        return new OkObjectResult(newEvent);
    }

}