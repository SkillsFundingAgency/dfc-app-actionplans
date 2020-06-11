using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Initializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Extensions
{
    public static class HostExtensions
    {
        public static IWebHost  AddNcsTelemetryInitializer(this IWebHost  host)
        {
            var telemetryConfig = (TelemetryConfiguration) host.Services.GetService(typeof(TelemetryConfiguration));
            var logger =
                (ILogger<NCSTelemetryInitializer>) host.Services.GetService(typeof(ILogger<NCSTelemetryInitializer>));
            telemetryConfig.TelemetryInitializers.Add(new NCSTelemetryInitializer(logger));

            return host;
        }
    }
}
