using Dfc.ProviderPortal.Packages;

using Microsoft.Extensions.Options;
using System;
using System.Collections;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;
using Interaction = DFC.App.ActionPlans.Services.DSS.Models.Interaction;

namespace DFC.App.ActionPlans.Services.DSS.Services
{
    public class DssService : IDssReader, IDssWriter
    {
        const string VersionHeader = "version";
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
                request.Headers.Add(VersionHeader, _dssSettings.Value.CustomerApiVersion);
                var result =  await _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}",
                    request);
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("Customer not found");
                
                return result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Customer Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<List<Session>> GetSessions(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();
            
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.SessionApiVersion);
                var result = await _restClient.GetAsync<List<Session>>(
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

        public async Task<Interaction> GetInteractionDetails(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();
            
            try
            {
                var result = await _restClient.GetAsync<Interaction>(
                    _dssSettings.Value.InteractionsApiUrl
                        .Replace("{customerId}", customerId)
                        .Replace("{interactionId}", interactionId),
                    request);
                
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("Interaction not found");
                
                return result;
            }   
            catch  (Exception e)
            {
                throw new DssException($"Failure InteractionDetails, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<List<Models.Action>> GetActions(string customerId, string interactionId, string actionPlanId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.ActionsApiVersion);
                var result = await _restClient.GetAsync<List<Models.Action>>(
                    _dssSettings.Value.ActionsApiUrl
                        .Replace("{customerId}", customerId)
                        .Replace("{interactionId}", interactionId)
                        .Replace("{actionPlanId}", actionPlanId),
                    request);
                return _restClient.LastResponse.StatusCode == HttpStatusCode.NoContent
                    ? new List<Models.Action>()
                    : result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Actions, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<List<Goal>> GetGoals(string customerId, string interactionId, string actionPlanId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.GoalsApiVersion);
                var result = await _restClient.GetAsync<List<Goal>>(
                    _dssSettings.Value.GoalsApiUrl
                        .Replace("{customerId}", customerId)
                        .Replace("{interactionId}", interactionId)
                        .Replace("{actionPlanId}", actionPlanId),
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

        public async Task<Adviser> GetAdviserDetails(string adviserId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.AdviserDetailsApiVersion);
                var result = await _restClient.GetAsync<Adviser>(
                    _dssSettings.Value.AdviserDetailsApiUrl
                        .Replace("{adviserDetailId}", adviserId), request);
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("Adviser Not found");
                
                return result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Adviser Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        private HttpRequestMessage CreateRequestMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            request.Headers.Add("TouchpointId", _dssSettings.Value.TouchpointId);

            return request;
        }

        public async Task UpdateActionPlan(UpdateActionPlan updateActionPlan)
        {
            
            if (updateActionPlan == null)
                throw new DssException($"Failure Update Action Plan, No data provided");

            try
            {

                ActionPlan result;
                using (var request = CreateRequestMessage())
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(updateActionPlan),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json);
                    request.Headers.Add(VersionHeader, _dssSettings.Value.ActionPlansApiVersion);

                    await _restClient.PatchAsync<ActionPlan>(_dssSettings.Value.ActionPlansApiUrl
                        .Replace("{customerId}",updateActionPlan.CustomerId.ToString())
                            .Replace("{interactionId}",updateActionPlan.InteractionId.ToString())
                                .Replace("{actionPlanId}",updateActionPlan.ActionPlanId.ToString())
                         , request);
                }

                if (!_restClient.LastResponse.IsSuccess)
                {
                    throw new DssException($"Failure Update Action Plan, No data provided -Response {_restClient.LastResponse.Content} ");
                }

                
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Update Action Plan, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
            
        }

        public async Task<ActionPlan> GetActionPlanDetails(string customerId, string interactionId, string actionPlanId)
        {
            
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.ActionPlansApiVersion);
                var result = await _restClient.GetAsync<ActionPlan>(_dssSettings.Value.ActionPlansApiUrl
                        .Replace("{customerId}",customerId)
                        .Replace("{interactionId}",interactionId)
                        .Replace("{actionPlanId}",actionPlanId)
                    , request);
                
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("Action Plan Not found");
                
                return result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Action Plan, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
                
            
        }

        public async Task<Goal> GetGoalDetails(string customerId, string interactionId, string actionPlanId, string goalId)
        {
            
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.GoalsApiVersion);
                var result = await _restClient.GetAsync<Goal>(_dssSettings.Value.GoalsApiUrl
                        .Replace("{customerId}",customerId)
                        .Replace("{interactionId}",interactionId)
                        .Replace("{actionPlanId}",actionPlanId) + "/" + goalId
                    , request);
                
                if (_restClient.LastResponse.StatusCode==HttpStatusCode.NoContent)
                    throw new DssException("Goal not found");
                
                return result;
            }
            catch (Exception e)
            {
                throw new DssException($"Failure Get Goal Details, Code:{_restClient.LastResponse.StatusCode} {Environment.NewLine}  {e.InnerException}");
            }
                
            
        }
    }
}