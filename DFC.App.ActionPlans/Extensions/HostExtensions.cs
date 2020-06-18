using System.Diagnostics.CodeAnalysis;
using DFC.App.ActionPlans.Initializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace DFC.App.ActionPlans.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class HostExtensions
    {
        public static IWebHost  AddNcsTelemetryInitializer(this IWebHost  host)
        {
            var telemetryConfig = (TelemetryConfiguration) host.Services.GetService(typeof(TelemetryConfiguration));
            var logger =
                (ILogger<NcsTelemetryInitializer>) host.Services.GetService(typeof(ILogger<NcsTelemetryInitializer>));
            telemetryConfig.TelemetryInitializers.Add(new NcsTelemetryInitializer(logger));

            return host;
        }
    }
}
