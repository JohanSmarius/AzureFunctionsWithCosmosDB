
using CosmosDBClientBased.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosDBClientBased;

public class FirstAidEventService
{
    private readonly CosmosClient cosmosClient;
    private readonly Container _container;

    public FirstAidEventService(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
        _container = cosmosClient.GetContainer("eventplanning", "events");
    }

    public async Task<List<FirstAidEvent>> GetEventsAsync()
    {
        var query = new QueryDefinition("SELECT * FROM c where c.IsDeleted = false");
        var iterator = _container.GetItemQueryIterator<FirstAidEvent>(query);
        var results = new List<FirstAidEvent>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.ToList());
        }
        return results;
    }

    // Create a get method for 1 event by id
    public async Task<FirstAidEvent> GetEventByIdAsync(Guid eventId)
    {
        try
        {
            var eventById = await _container.ReadItemAsync<FirstAidEvent>(eventId.ToString(), new PartitionKey(eventId.ToString()));
            return eventById.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task AddEventAsync(FirstAidEvent newEvent)
    {
        newEvent.id = Guid.NewGuid();
        await _container.CreateItemAsync(newEvent, new PartitionKey(newEvent.id.ToString()));
    }

    public async Task UpdateEventAsync(FirstAidEvent updatedEvent)
    {
        await _container.UpsertItemAsync(updatedEvent, new PartitionKey(updatedEvent.id.ToString()));
    }

    public async Task SoftDeleteEventAsync(Guid eventId)
    {
        var eventToDelete = await _container.ReadItemAsync<FirstAidEvent>(eventId.ToString(), new PartitionKey(eventId.ToString()));
        if (eventToDelete != null)
        {
            eventToDelete.Resource.IsDeleted = true;
            await _container.UpsertItemAsync(eventToDelete.Resource, new PartitionKey(eventId.ToString()));
        }
    }
}
