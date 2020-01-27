using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Dfc.App.ActionPlans.IntegrationTests.Controllers
{
    [TestFixture]
    public class HomeControllerIntegrationTests : IntegrationTestBase
    {
        [TestFixture]
        public class Index
        {
            [Test]
            public async Task When_GetRootHTML_Then_Return200OKWithHTML()
            {
                // Arrange.
                var url = "/";

                // Act.
                var result = await HttpClient.GetAsync(url);

                // Assert.
                result.Should().NotBeNull();
                result.StatusCode.Should().Be(StatusCodes.Status200OK);
                var content = await result.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrWhiteSpace();
            }
        }
    }
}
