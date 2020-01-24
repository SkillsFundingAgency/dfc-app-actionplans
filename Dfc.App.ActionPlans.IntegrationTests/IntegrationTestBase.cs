using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Dfc.App.ActionPlans.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        private class TestWebApplicationFactory : WebApplicationFactory<Startup>
        {
        }

        protected static HttpClient HttpClient { get; }

        static IntegrationTestBase()
        {
            var factory = new TestWebApplicationFactory();
            HttpClient = factory.CreateClient();
        }
    }
}
