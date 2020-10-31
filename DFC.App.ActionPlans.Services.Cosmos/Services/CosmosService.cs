using System;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Models;
using Microsoft.Azure.Cosmos;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dfc.ProviderPortal.Packages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Cosmos.Services
{
 
    public enum CosmosCollection
    {
        Session = 1,
        Content = 2
    }

    public class CosmosService : ICosmosService
    {
        private readonly CosmosSettings _settings;
        private readonly CosmosClient _client;
        private readonly ILogger<CosmosService> _logger;
        public CosmosService(IOptions<CosmosSettings> settings, CosmosClient client, ILogger<CosmosService> logger)
        {
            
            _settings = settings.Value;
            _client = client;
            _logger = logger;
        }
        public async Task<HttpResponseMessage> CreateItemAsync(object item, CosmosCollection collection)
        {
            Throw.IfNull(item, nameof(item));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));

            var result =  await container.CreateItemAsync(item);

            if (result.StatusCode == HttpStatusCode.Created) 
                return new HttpResponseMessage(HttpStatusCode.Created);

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        public async Task<HttpResponseMessage> ReadItemAsync(string id, string partitionKey, CosmosCollection collection)
        {
            Throw.IfNullOrWhiteSpace(id, nameof(id));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));
            try
            {
                var result = await container.ReadItemAsync<object>(id, string.IsNullOrEmpty(partitionKey) ? PartitionKey.None : new PartitionKey(partitionKey));

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    if(result.Resource == null)
                        return new HttpResponseMessage(HttpStatusCode.NotFound);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(result.Resource.ToString()) 
                    };
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to find item with id: {id}, Error message: {e.Message}, Inner Error: {e.InnerException?.Message}");
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        public async Task<HttpResponseMessage> UpsertItemAsync(object item, CosmosCollection collection)
        {
            Throw.IfNull(item, nameof(item));

            var container = _client.GetContainer(_settings.DatabaseName, GetContainerName(collection));
            Throw.IfNull(container, nameof(container));

            var result = await container.UpsertItemAsync(item);
            return result.StatusCode switch
            {
                HttpStatusCode.OK => new HttpResponseMessage(HttpStatusCode.OK),
                HttpStatusCode.Created => new HttpResponseMessage(HttpStatusCode.Created),
                _ => new HttpResponseMessage(HttpStatusCode.InternalServerError)
            };
        }

        internal string GetContainerName(CosmosCollection collection)
        {
            return collection switch
            {
                CosmosCollection.Session => _settings.UserSessionsCollection,
                CosmosCollection.Content => _settings.ContentCollection,
                _ => _settings.UserSessionsCollection
            };
        }
    }
}
