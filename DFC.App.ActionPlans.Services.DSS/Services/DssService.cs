using Dfc.ProviderPortal.Packages;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
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
            request.Headers.Add("version", _dssSettings.Value.CustomerApiVersion);

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

        private HttpRequestMessage CreateRequestMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("Ocp-Apim-Subscription-Key", _dssSettings.Value.ApiKey);
            request.Headers.Add("TouchpointId", _dssSettings.Value.TouchpointId);

            return request;
        }

       

    }
}