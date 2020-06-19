using System;
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
    public abstract class DssTests
    {
        protected IDssReader DssService;
        protected RestClient RestClient;
        protected ILogger<DssService> Logger;
        protected IOptions<DssSettings> DssSettings;

        
        public void Setup(string dssSuccess)
        {
            Logger = Substitute.For<ILogger<DssService>>();
            var mockHandler = DssHelpers.GetMockMessageHandler(dssSuccess,
                statusToReturn: HttpStatusCode.Created);
            RestClient = new RestClient(mockHandler.Object);
            DssSettings = Options.Create(new DssSettings()
            {
                ApiKey = "9238dfjsjdsidfs83fds",
                SessionApiUrl = "https://this.is.anApi.org.uk",
                CustomerApiVersion = "V3",
                CustomerApiUrl = "https://this.is.anApi.org.uk",
                SessionApiVersion = "V3",
                GoalsApiUrl = "https://this.is.anApi.org.uk",
                GoalsApiVersion = "V2",
                ActionsApiUrl= "https://this.is.anApi.org.uk",
                ActionsApiVersion= "v3",
                InteractionsApiUrl = "https://this.is.anApi.org.uk",
                AdviserDetailsApiVersion = "v2",
                AdviserDetailsApiUrl = "https://this.is.anApi.org.uk",
                TouchpointId = "9000000001"
            });
            DssService = new DssService(RestClient, DssSettings, Logger);
        }
    }

    public class DssServiceTests
    {
        public class GetSessionTests : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssSessionDetails());
            }
            [Test]
            public async Task When_GetSessionData_ReturnSession()
            {
                var result = await DssService.GetSessions("customer", "interaction");
                result.Should().NotBeNull();
            }

            [Test]
            public async Task When_GetSessionWithNoContent_Return_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                DssService.Invoking(sut => sut.GetSessions("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().Throw<DssException>();
            }
        }
        public class GetGoalsTests : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssGoalsList());
            }
            [Test]
            public async Task When_GetGoalsData_ReturnGoalsList()
            {
                var result = await DssService.GetGoals("customer", "interactionid","actionplanid");
                result.Count.Should().Be(4);
            }

            [Test]
            public async Task When_GetGoalsDataWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                var result = await DssService.GetGoals("customer", "interactionid","actionplanid");
                result.Count.Should().Be(0);
                
            }
        }
        public class GetActionsTests : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssActionsList());
            }
            [Test]
            public async Task When_GetActionsData_ReturnGetActionsList()
            {
                var result = await DssService.GetActions("customer", "interactionid","actionplanid");
                result.Count.Should().Be(3);
            }

            [Test]
            public async Task When_GetActionsWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                var result = await DssService.GetActions("customer", "interactionid","actionplanid");
                result.Count.Should().Be(0);
                
            }
        }
        public class GetInteractionsTests : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssInteractionDetails());
            }
            [Test]
            public async Task When_GetGetInteractionDetails_ReturnInterationDetails()
            {
                var result = await DssService.GetInteractionDetails("customer", "interactionid");
                result.Should().NotBe(null);
            }
            [Test]
            public async Task When_GetInteractionDetailsWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);

                DssService.Invoking(sut => sut.GetInteractionDetails("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().Throw<DssException>();

            }
        }
        public class GetAdviserDetails: DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssAdviserDetails());
            }
            [Test]
            public async Task When_GetAdviserDetails_ReturnAdviserDetails()
            {
                var result = await DssService.GetAdviserDetails("adverid");
                result.Should().NotBe(null);
            }
            [Test]
            public async Task When_AdviserDetailsWithNoContent_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);

                DssService.Invoking(sut => sut.GetAdviserDetails("993cfb94-12b7-41c4-b32d-7be9331174f1"))
                    .Should().Throw<DssException>();

            }
        }
        public class GetGoalDetails: DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssGoalDetails());
            }
            [Test]
            public async Task When_GetGoalDetails_ReturnGoalDetails()
            {
                var result = await DssService.GetGoalDetails("customerId", "interactionId2","actionPlanId","goalId");
                result.Should().NotBe(null);
            }
            [Test]
            public async Task When_AdviserDetailsWithNoContent_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                DssService.Invoking(sut => sut.GetGoalDetails("customerId", "interactionId2","actionPlanId","goalId"))
                    .Should().Throw<DssException>();
            }
        }
        public class UpdateActionPlan: DssTests
        {
            private IDssWriter _dssWriter;   
            ActionPlans.Services.DSS.Models.UpdateActionPlan updateActionPlan;
                
            [SetUp]
            public void Init()
            {
                
                Logger = Substitute.For<ILogger<DssService>>();
                var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulUpdateActionPlan(),
                    statusToReturn: HttpStatusCode.Created);
                RestClient = new RestClient(mockHandler.Object);
                DssSettings = Options.Create(new DssSettings()
                {
                    ApiKey = "9238dfjsjdsidfs83fds",
                    SessionApiUrl = "https://this.is.anApi.org.uk",
                    CustomerApiVersion = "V3",
                    CustomerApiUrl = "https://this.is.anApi.org.uk",
                    SessionApiVersion = "V3",
                    GoalsApiUrl = "https://this.is.anApi.org.uk",
                    GoalsApiVersion = "V2",
                    ActionsApiUrl= "https://this.is.anApi.org.uk",
                    ActionsApiVersion= "v3",
                    InteractionsApiUrl = "https://this.is.anApi.org.uk",
                    AdviserDetailsApiVersion = "v2",
                    ActionPlansApiUrl = "https://this.is.anApi.org.uk",
                    ActionPlansApiVersion = "v2",
                    AdviserDetailsApiUrl = "https://this.is.anApi.org.uk",
                    TouchpointId = "9000000001"
                });
                _dssWriter = new DssService(RestClient, DssSettings, Logger);
                updateActionPlan = new ActionPlans.Services.DSS.Models.UpdateActionPlan()
                {
                    CustomerId = new Guid(),
                    InteractionId = new Guid(),
                    ActionPlanId = new Guid(),
                    DateActionPlanAcknowledged = DateTime.Now
                };
            }

            
            [Test]
            public async Task When_UpdateActionPlan_ReturnActionPlan()
            {
                await _dssWriter.UpdateActionPlan(updateActionPlan);
            }

            [Test]
            public async Task When_UpdateActionPlanNoSuccess_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                _dssWriter.Invoking(sut => sut.UpdateActionPlan(updateActionPlan))
                    .Should().Throw<DssException>();

            }

        }
    }
}