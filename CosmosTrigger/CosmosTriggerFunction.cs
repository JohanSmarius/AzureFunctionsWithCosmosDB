using System;
using System.Collections.Generic;
using CosmosTrigger.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CosmosTrigger
{
    public class CosmosTriggerFunction
    {
        private readonly ILogger _logger;

        public CosmosTriggerFunction(ILogger<CosmosTriggerFunction> logger)
        {
            _logger = logger;
        }

        [Function("CosmosTrigger")]
        public void Run([CosmosDBTrigger(
            databaseName: "eventplanning",
            containerName: "events",
            Connection = "CosmosDBConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<FirstAidEvent> input)
        {
            if (input != null && input.Count > 0)
            {
                foreach (var item in input)
                {
                    Console.WriteLine($"Added: {item.Name}");
                }
            }
        }
    }

}
