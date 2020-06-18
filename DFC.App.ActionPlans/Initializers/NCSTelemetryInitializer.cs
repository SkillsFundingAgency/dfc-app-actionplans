using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Initializers
{
        [ExcludeFromCodeCoverage]
        public class NcsTelemetryInitializer : ITelemetryInitializer
        {
            private readonly ILogger<NcsTelemetryInitializer> _logger;
            private string _applicationName;
            private string _applicationInstanceId;

            public NcsTelemetryInitializer(ILogger<NcsTelemetryInitializer> logger)
            {
                this._logger = logger;
                Setup();
            }

            public void Setup()
            {
                _applicationInstanceId = Guid.NewGuid().ToString();
                _applicationName = Assembly.GetExecutingAssembly().GetName().Name;

                //Log to Console for App Service / K8S Tracing
                Console.WriteLine($"Application Name: {_applicationName}");
                Console.WriteLine($"Application Instance Id: {_applicationInstanceId}");

                _logger.LogInformation($"Application Name: {_applicationName}");
                _logger.LogInformation($"Application Instance Id: {_applicationInstanceId}");
            }

            public void Initialize(ITelemetry telemetry)
            {
                //RoleName is used to distinguish instances in the AI Application Map
                //Pods in K8S will have a null value, so set to the instance ID 
                if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
                {
                    telemetry.Context.Cloud.RoleName = $"{_applicationName}_{_applicationInstanceId}";
                }

                //Add to Custom Properties in AI to allow correlation
                if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationName"))
                {
                    telemetry.Context.GlobalProperties.Add("ApplicationName", _applicationName);
                }
                if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationInstanceId"))
                {
                    telemetry.Context.GlobalProperties.Add("ApplicationInstanceId", _applicationInstanceId.ToString());
                }
            }
        }
    }

