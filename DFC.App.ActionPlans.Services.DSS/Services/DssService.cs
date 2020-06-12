using Dfc.ProviderPortal.Packages;

using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Services.DSS.Services
{
    public class DssService : IDssReader
    {
        private readonly IRestClient _restClient;
        private readonly IOptions<DssSettings> _dssSettings;
        private readonly ILogger<DssService> _logger;

        public DssService(IOptions<DssSettings> settings, ILogger<DssService> logger)
        {
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiUrl, nameof(settings.Value.CustomerApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiVersion, nameof(settings.Value.CustomerApiVersion));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.TouchpointId, nameof(settings.Value.TouchpointId));

            _restClient = new RestClient();
            _dssSettings = settings;
            _logger = logger;
        }

        public DssService(IRestClient restClient, IOptions<DssSettings> settings, ILogger<DssService> logger)
        {
            Throw.IfNull(restClient, nameof(restClient));
            Throw.IfNull(settings, nameof(settings));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiUrl, nameof(settings.Value.CustomerApiUrl));
            Throw.IfNullOrWhiteSpace(settings.Value.ApiKey, nameof(settings.Value.ApiKey));
            Throw.IfNullOrWhiteSpace(settings.Value.TouchpointId, nameof(settings.Value.TouchpointId));
            Throw.IfNullOrWhiteSpace(settings.Value.CustomerApiVersion, nameof(settings.Value.CustomerApiVersion));
            _restClient = restClient;
            _dssSettings = settings;
            _logger = logger;
        }


        public async Task<Customer> GetCustomerDetails(string customerId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                return await _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}",
                    request);
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Customer Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<IList<Session>> GetSessions(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();
            
            try
            {
                request.Headers.Add("version", _dssSettings.Value.SessionApiVersion);
                var result = await _restClient.GetAsync<IList<Session>>(
                    _dssSettings.Value.SessionApiUrl
                        .Replace("{customerId}", customerId)
                        .Replace("{interactionId}", interactionId),
                    request);
                
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("No sessions found");
                
                return result;
            }   
            catch  (Exception e)
            {
                throw new DssException($"Failure GetSessions, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<Interaction> InteractionDetails(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();
            
            try
            {
                request.Headers.Add("version", _dssSettings.Value.InteractionsApiVersion);
                var result = await _restClient.GetAsync<Interaction>(
                    _dssSettings.Value.InteractionsApiUrl
                        .Replace("{customerId}", customerId)
                        .Replace("{interactionId}", interactionId),
                    request);
                
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("No sessions found");
                
                return result;
            }   
            catch  (Exception e)
            {
                throw new DssException($"Failure InteractionDetails, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }


        public async Task<Customer> GetActions(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();

            try
            {
                request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);
                return await _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}",
                    request);
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Customer Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<IList<Goal>> GetGoals(string customerId, string interactionId, string actionPlanId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add("version", _dssSettings.Value.GoalApiVersion);
                var result =  await _restClient.GetAsync<IList<Goal>>($"{_dssSettings.Value.GoalApiUrl}{customerId}",
                    request);
                return _restClient.LastResponse.StatusCode == HttpStatusCode.NoContent
                    ? new List<Goal>()
                    : result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Goals, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        private HttpRequestMessage CreateRequestMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            request.Headers.Add("TouchpointId", _dssSettings.Value.TouchpointId);

            return request;
        }

       

    }
}