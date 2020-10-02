using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Services;

namespace DFC.App.ActionPlans.Cosmos.Interfaces
{
    public interface ICosmosReader
    {
        Task<HttpResponseMessage> ReadItemAsync(string id, string partitionKey, CosmosCollection collection);
    }
}
