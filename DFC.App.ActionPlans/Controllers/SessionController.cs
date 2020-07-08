using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using Microsoft.AspNetCore.Mvc;

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
                
                var result = await _cosmosService.CreateItemAsync(userSession, CosmosCollection.Session);
            }
        /*
            protected async Task<HttpResponseMessage> UpdateUserSession(string currentPage, UserSession session = null)
            {
                if (session == null)
                {
                    session = await _sessionService.GetUserSession();
                }
            
                session.PreviousPage = session.CurrentPage;
                session.CurrentPage = currentPage;
                session.LastUpdatedUtc = DateTime.UtcNow;

                return await _sessionService.UpdateUserSessionAsync(session);
            }

            protected async Task<UserSession> GetUserSession()
            {
                return await _sessionService.GetUserSession();
            }

            protected async Task<UserSession> GetUserSession(string code)
            {
                return await _sessionService.Reload(GetSessionId(code));
            }


            public string GetSessionId(string code)
            {
                var result = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(code))
                {
                    code = code.ToLower();
                    foreach (var c in code)
                    {
                        if (c != ' ')
                        {
                            result.Append(c.ToString());
                        }
                    }
                }

                return result.ToString();
            }
        }*/
    }
}
