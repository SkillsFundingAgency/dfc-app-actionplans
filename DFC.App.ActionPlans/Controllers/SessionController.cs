using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DFC.App.ActionPlans.Controllers
{
    public abstract class SessionController : Controller
    {
        private readonly ICosmosService _cosmosService;

        protected SessionController(ICosmosService cosmosServiceService)
        {
            _cosmosService = cosmosServiceService;
        }

        protected async Task CreateUserSession(UserSession userSession)
        {
            await _cosmosService.CreateItemAsync(userSession, CosmosCollection.Session);
        }


        protected async Task<UserSession> GetUserSession(string Id, string partitionKey)
        {
            var result = await _cosmosService.ReadItemAsync(Id, partitionKey, CosmosCollection.Session);
            return result.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync())
                : null;
        }
    }
}