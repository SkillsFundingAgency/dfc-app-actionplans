using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Dfc.App.ActionPlans;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args)
    {
        var webHost = WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

        return webHost;
    }
}