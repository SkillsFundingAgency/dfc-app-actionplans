using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.UnitTest.Helpers;
using DFC.App.ActionPlans.Services.DSS;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.Personalisation.Common.Net.RestClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace DFC.App.Account.Services.DSS.UnitTest
{
    public abstract class DSSTests
    {
        protected IDssReader _dssService;
        protected RestClient _restClient;
        protected ILogger<DssService> _logger;
        protected IOptions<DssSettings> _dssSettings;

        
        public void Setup(string dssSuccess)
        {
            _logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(dssSuccess,
                statusToReturn: HttpStatusCode.Created);
            _restClient = new RestClient(mockHandler.Object);
            _dssSettings = Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                SessionApiUrl = "https://this.is.anApi.org.uk",
                CustomerApiVersion = "V3",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                SessionApiVersion = "V3",
                GoalApiUrl = "https://this.is.anApi.org.uk",
                GoalApiVersion = "V2",
                ActionsApiUrl= "https://this.is.anApi.org.uk",
                ActionsApiVersion= "v3",
                TouchpointId = "9000000001"
            });
            _dssService = new DssService(_restClient, _dssSettings, _logger);
        }
    }

    public class DssServiceTests
    {
        
        public class GetSessionTests : DSSTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDSSSessionDetails());
            }
            [Test]
            public async Task When_GetSessionData_ReturnSession()
            {
                var result = await _dssService.GetSessions("customer", "interaction");
                result.Should().NotBeNull();
            }

            [Test]
            public async Task When_GetSessionWithNoContent_Return_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                _dssService = new DssService(restClient, _dssSettings, _logger);
                _dssService.Invoking(sut => sut.GetSessions("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().Throw<DssException>();
            }
        }

        public class GetGoalsTests : DSSTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDSSGoalsList());
            }
            [Test]
            public async Task When_GetGoalsData_ReturnGoalsList()
            {
                var result = await _dssService.GetGoals("customer", "interactionid","actionplanid");
                result.Count.Should().Be(4);
            }

            [Test]
            public async Task When_GetGoalsDataWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                _dssService = new DssService(restClient, _dssSettings, _logger);
                var result = await _dssService.GetGoals("customer", "interactionid","actionplanid");
                result.Count.Should().Be(0);
                
            }
        }

        public class GetActionsTests : DSSTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDSSActionsList());
            }
            [Test]
            public async Task When_GetActionsData_ReturnGetActionsList()
            {
                var result = await _dssService.GetActions("customer", "interactionid","actionplanid");
                result.Count.Should().Be(3);
            }

            [Test]
            public async Task When_GetActionsWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                _dssService = new DssService(restClient, _dssSettings, _logger);
                var result = await _dssService.GetActions("customer", "interactionid","actionplanid");
                result.Count.Should().Be(0);
                
            }
        }
    }
}