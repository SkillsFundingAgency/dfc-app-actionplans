﻿using Dfc.ProviderPortal.Packages;

using Microsoft.Extensions.Options;
using System;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;
using Interaction = DFC.App.ActionPlans.Services.DSS.Models.Interaction;

namespace DFC.App.ActionPlans.Services.DSS.Services
{
    [ExcludeFromCodeCoverage]
    public class DssService : IDssReader, IDssWriter
    {
        const string VersionHeader = "version";
        const string CustomerIdTag = "{customerId}";
        const string InteractionIdTag = "{interactionId}";
        const string ActionPlanIdTag = "{actionPlanId}";

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
                var result = await _restClient.GetAsync<Customer>($"{_dssSettings.Value.CustomerApiUrl}{customerId}",
                    request);
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent || result.CustomerId == Guid.Empty)
                {
                    var errorMessage = "Customer not found";
                    _logger.LogError(errorMessage);
                    throw new DssException("Customer not found");
                }

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Customer Details, Code:{_restClient.LastResponse?.StatusCode} {Environment.NewLine}  CustomerId:{customerId} {Environment.NewLine} {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
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
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId),
                    request);

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    throw new DssException("No sessions found");

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure GetSessions, Code:{_restClient.LastResponse?.StatusCode} {Environment.NewLine} CustomerId:{customerId} InteractionId:{interactionId} {Environment.NewLine} {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

        }

        public async Task<Interaction> GetInteractionDetails(string customerId, string interactionId)
        {
            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, "");
                var result = await _restClient.GetAsync<Interaction>(
                    _dssSettings.Value.InteractionsApiUrl
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId),
                    request);

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var errorMessage = $"Interaction not found  CustomerId:{customerId} InteractionId:{interactionId} ";
                    _logger.LogError(errorMessage);
                    throw new DssException(errorMessage);
                }

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure InteractionDetails, CustomerId:{customerId} InteractionId:{interactionId} {Environment.NewLine} {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
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
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId)
                        .Replace(ActionPlanIdTag, actionPlanId),
                    request);
                return _restClient.LastResponse.StatusCode == HttpStatusCode.NoContent
                    ? new List<Models.Action>()
                    : result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Actions, CustomerId:{customerId} InteractionId:{interactionId} ActionPlanId{actionPlanId} {Environment.NewLine} {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
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
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId)
                        .Replace(ActionPlanIdTag, actionPlanId),
                    request);
                return _restClient.LastResponse.StatusCode == HttpStatusCode.NoContent
                    ? new List<Goal>()
                    : result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Goals, CustomerId:{customerId} InteractionId:{interactionId} ActionPlanId{actionPlanId} {Environment.NewLine} {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
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
                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var errorMessage = "Adviser Not found";
                    _logger.LogError(errorMessage);
                    return null;
                }

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Adviser Details, AdviserId{adviserId} {Environment.NewLine}  {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

        }

        public async Task<ActionPlan> GetActionPlanDetails(string customerId, string interactionId, string actionPlanId)
        {

            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.ActionPlansApiVersion);
                var result = await _restClient.GetAsync<ActionPlan>(_dssSettings.Value.ActionPlansApiUrl
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId)
                        .Replace(ActionPlanIdTag, actionPlanId)
                    , request);

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    throw new DssException("Action Plan Not found");

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Action Plan, CustomerId:{customerId} InteractionId:{interactionId} ActionPlanId{actionPlanId} {Environment.NewLine}  {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }


        }

        public async Task<Goal> GetGoalDetails(string customerId, string interactionId, string actionPlanId, string goalId)
        {

            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.GoalsApiVersion);
                var result = await _restClient.GetAsync<Goal>(_dssSettings.Value.GoalsApiUrl
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId)
                        .Replace(ActionPlanIdTag, actionPlanId) + "/" + goalId
                    , request);

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                    throw new DssException("Goal not found");

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Goal Details, {Environment.NewLine} CustomerId:{customerId} InteractionId:{interactionId} ActionPlanId{actionPlanId} GoalId{goalId} {Environment.NewLine}  {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }


        }

        public async Task<Action> GetActionDetails(string customerId, string interactionId, string actionPlanId, string goalId)
        {

            var request = CreateRequestMessage();
            try
            {
                request.Headers.Add(VersionHeader, _dssSettings.Value.ActionsApiVersion);
                var result = await _restClient.GetAsync<Action>(_dssSettings.Value.ActionsApiUrl
                        .Replace(CustomerIdTag, customerId)
                        .Replace(InteractionIdTag, interactionId)
                        .Replace(ActionPlanIdTag, actionPlanId) + "/" + goalId
                    , request);

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var errorMessage = "Action not found";
                    _logger.LogError(errorMessage);
                    throw new DssException(errorMessage);
                }

                return result;
            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Get Action Details, CustomerId:{customerId} InteractionId:{interactionId} ActionPlanId{actionPlanId} GoalId{goalId} {Environment.NewLine}  {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }
        }

        public async Task UpdateActionPlan(UpdateActionPlan updateActionPlan)
        {

            if (updateActionPlan == null)
            {
                var errorMessage = $"Failure Update Action Plan, No data provided";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

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
                            .Replace(CustomerIdTag, updateActionPlan.CustomerId.ToString())
                            .Replace(InteractionIdTag, updateActionPlan.InteractionId.ToString())
                            .Replace(ActionPlanIdTag, updateActionPlan.ActionPlanId.ToString())
                        , request);
                }

                if (!_restClient.LastResponse.IsSuccess)
                {
                    var errorMessage = $"Failure Update Action Plan - Response {_restClient.LastResponse.Content} ";
                    _logger.LogError(errorMessage);
                    throw new DssException(errorMessage);
                }


            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Update Action Plan, {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

        }

        public async Task UpdateGoal(UpdateGoal updateGoal)
        {

            if (updateGoal == null)
            {
                var errorMessage = $"Failure Update Goal, No data provided";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

            try
            {

                ActionPlan result;
                using (var request = CreateRequestMessage())
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(updateGoal),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json);
                    request.Headers.Add(VersionHeader, _dssSettings.Value.GoalsApiVersion);

                    await _restClient.PatchAsync<Goal>(_dssSettings.Value.GoalsApiUrl
                            .Replace(CustomerIdTag, updateGoal.CustomerId.ToString())
                            .Replace(InteractionIdTag, updateGoal.InteractionId.ToString())
                            .Replace(ActionPlanIdTag, updateGoal.ActionPlanId.ToString()) + "/" + updateGoal.GoalId
                        , request);
                }

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var errorMessage = $"Failure Update Goal - Response {_restClient.LastResponse.Content} ";
                    _logger.LogError(errorMessage);
                    throw new DssException(errorMessage);
                }


            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Update Goal,  {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

        }

        public async Task UpdateAction(UpdateAction updateAction)
        {

            if (updateAction == null)
            {
                var errorMessage = $"Failure Update Action, No data provided";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
            }

            try
            {

                ActionPlan result;
                using (var request = CreateRequestMessage())
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(updateAction),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json);
                    request.Headers.Add(VersionHeader, _dssSettings.Value.ActionsApiVersion);

                    await _restClient.PatchAsync<Goal>(_dssSettings.Value.ActionsApiUrl
                            .Replace(CustomerIdTag, updateAction.CustomerId.ToString())
                            .Replace(InteractionIdTag, updateAction.InteractionId.ToString())
                            .Replace(ActionPlanIdTag, updateAction.ActionPlanId.ToString()) + "/" + updateAction.ActionId
                        , request);
                }

                if (_restClient.LastResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var errorMessage = $"Failure Update Action - Response {_restClient.LastResponse.Content} ";
                    _logger.LogError(errorMessage);
                    throw new DssException(errorMessage);
                }


            }
            catch (Exception e)
            {
                var errorMessage = $"Failure Update Action, {e.InnerException}";
                _logger.LogError(errorMessage);
                throw new DssException(errorMessage);
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