using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DFC.App.ActionPlans.Controllers
{
    public abstract class SessionController : Controller
    {
        private readonly ICosmosService _cosmosService;
      //  private readonly ILogger _dsslogger;

        protected SessionController(ICosmosService cosmosServiceService)
        {
            _cosmosService = cosmosServiceService;
            //_dsslogger = logger;
        }

        protected async Task CreateUserSession(UserSession userSession)
        {
            //_dsslogger.LogInformation($"SessionController CreateUserSession CustomerId {userSession.CustomerId} ");
            await _cosmosService.CreateItemAsync(userSession, CosmosCollection.Session);
        }

        protected async Task UpdateSession(UserSession session)
        {
            //_dsslogger.LogInformation($"SessionController UpdateSession CustomerId {session.CustomerId} ");
            await _cosmosService.UpsertItemAsync(session, CosmosCollection.Session);
        }

        protected async Task<UserSession> GetUserSession(string Id, string partitionKey = "")
        {
            if (string.IsNullOrEmpty(Id))
            {
                //_dsslogger.LogInformation($"SessionController GetUserSession Id is null ");
                return null;
            }
            var result = await _cosmosService.ReadItemAsync(Id, partitionKey, CosmosCollection.Session);
            //_dsslogger.LogInformation($"SessionController GetUserSession result.IsSuccessStatusCode Id {Id} ");
            return result.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<UserSession>(await result.Content.ReadAsStringAsync())
                : null;
        }
    }
}