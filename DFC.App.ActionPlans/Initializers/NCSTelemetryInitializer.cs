using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Initializers
{
        [ExcludeFromCodeCoverage]
        public class NCSTelemetryInitializer : ITelemetryInitializer
        {
            private readonly ILogger<NCSTelemetryInitializer> _logger;
            private string applicationName;
            private string applicationInstanceId;

            public NCSTelemetryInitializer(ILogger<NCSTelemetryInitializer> logger)
            {
                this._logger = logger;
                Setup();
            }

            public void Setup()
            {
                applicationInstanceId = Guid.NewGuid().ToString();
                applicationName = Assembly.GetExecutingAssembly().GetName().Name;

                //Log to Console for App Service / K8S Tracing
                Console.WriteLine($"Application Name: {applicationName}");
                Console.WriteLine($"Application Instance Id: {applicationInstanceId}");

                _logger.LogInformation($"Application Name: {applicationName}");
                _logger.LogInformation($"Application Instance Id: {applicationInstanceId}");
            }

            public void Initialize(ITelemetry telemetry)
            {
                //RoleName is used to distinguish instances in the AI Application Map
                //Pods in K8S will have a null value, so set to the instance ID 
                if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
                {
                    telemetry.Context.Cloud.RoleName = $"{applicationName}_{applicationInstanceId}";
                }

                //Add to Custom Properties in AI to allow correlation
                if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationName"))
                {
                    telemetry.Context.GlobalProperties.Add("ApplicationName", applicationName);
                }
                if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationInstanceId"))
                {
                    telemetry.Context.GlobalProperties.Add("ApplicationInstanceId", applicationInstanceId.ToString());
                }
            }
        }
    }

