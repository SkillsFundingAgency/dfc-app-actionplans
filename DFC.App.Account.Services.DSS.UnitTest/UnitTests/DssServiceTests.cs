using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.App.ActionPlans.Services.DSS.UnitTest.Helpers;
using DFC.Personalisation.Common.Net.RestClient;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.Services.DSS.UnitTest.UnitTests
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
                ActionsApiUrl = "https://this.is.anApi.org.uk",
                ActionsApiVersion = "v3",
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
                _ = await DssService.Invoking(sut => sut.GetSessions("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_GetSessionErrors_Return_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.GetAsync<List<Session>>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));
                DssService = new DssService(restClient, DssSettings, Logger);
                _ = await DssService.Invoking(sut => sut.GetSessions("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
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
                var result = await DssService.GetGoals("customer", "interactionid", "actionplanid");
                result.Count.Should().Be(4);
            }

            [Test]
            public async Task When_GetGoalsDataWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                var result = await DssService.GetGoals("customer", "interactionid", "actionplanid");
                result.Count.Should().Be(0);

            }

            [Test]
            public async Task When_GetGoalsDataWithNoContent_Return_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);

                restClient.GetAsync<List<Goal>>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));

                _ = await DssService.Invoking(sut => sut.GetGoals("customer", "interactionid", "actionplanid"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
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
                var result = await DssService.GetActions("customer", "interactionid", "actionplanid");
                result.Count.Should().Be(3);
            }

            [Test]
            public async Task When_GetActionsWithNoContent_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                var result = await DssService.GetActions("customer", "interactionid", "actionplanid");
                result.Count.Should().Be(0);

            }

            [Test]
            public async Task When_GetActionsErrors_Return_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                restClient.GetAsync<List<Action>>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));

                _ = await DssService.Invoking(sut => sut.GetActions("customer", "interactionid", "actionplanid"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);

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

                _ = await DssService.Invoking(sut => sut.GetInteractionDetails("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_GetInteractionDetailsErrors_Return_EmptyList()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                restClient.GetAsync<Interaction>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));
                _ = await DssService.Invoking(sut => sut.GetInteractionDetails("993cfb94-12b7-41c4-b32d-7be9331174f1", "saddasdsadsa"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }
        }
        public class GetAdviserDetails : DssTests
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
            public async Task When_GoalDetailsWithNoContent_ReturnsNull()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);

                var result = await DssService.GetAdviserDetails("993cfb94-12b7-41c4-b32d-7be9331174f1");
                result.Should().Be(null);
            }

            [Test]
            public async Task When_GoalDetailsErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                restClient.GetAsync<Adviser>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));

                _ = await DssService.Invoking(sut => sut.GetAdviserDetails("993cfb94-12b7-41c4-b32d-7be9331174f1"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }
        }

        public class GetActionDetails : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssActionDetails());
            }
            [Test]
            public async Task When_GetActionDetails_ReturnActionDetails()
            {
                var result = await DssService.GetActionDetails("customerId", "interactionId2", "actionPlanId", "goalId");
                result.Should().NotBe(null);
            }
            [Test]
            public async Task When_ActionDetailsWithNoContent_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                _ = await DssService.Invoking(sut => sut.GetActionDetails("customerId", "interactionId2", "actionPlanId", "actionId"))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_ActionDetailsErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                restClient.GetAsync<Action>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));

                _ = await DssService.Invoking(sut => sut.GetActionDetails("customerId", "interactionId2", "actionPlanId", "actionId"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }
        }
        public class GetGoalDetails : DssTests
        {
            [SetUp]
            public void Init()
            {
                base.Setup(DssHelpers.SuccessfulDssGoalDetails());
            }
            [Test]
            public async Task When_GetGoalDetails_ReturnGoalDetails()
            {
                var result = await DssService.GetGoalDetails("customerId", "interactionId2", "actionPlanId", "goalId");
                result.Should().NotBe(null);
            }
            [Test]
            public async Task When_AdviserDetailsWithNoContent_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                _ = await DssService.Invoking(sut => sut.GetGoalDetails("customerId", "interactionId2", "actionPlanId", "goalId"))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_AdviserDetailsErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent));
                DssService = new DssService(restClient, DssSettings, Logger);
                restClient.GetAsync<Goal>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>()).ThrowsForAnyArgs(new Exception("error"));
                _ = await DssService.Invoking(sut => sut.GetGoalDetails("customerId", "interactionId2", "actionPlanId", "goalId"))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }
        }
        public class UpdateActionPlan : DssTests
        {
            private IDssWriter _dssWriter;
            private ActionPlans.Services.DSS.Models.UpdateActionPlan updateActionPlan;

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
                    ActionsApiUrl = "https://this.is.anApi.org.uk",
                    ActionsApiVersion = "v3",
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
            public Task When_UpdateActionPlan_ReturnActionPlan()
            {
                return _dssWriter.UpdateActionPlan(updateActionPlan);
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

                _ = await _dssWriter.Invoking(sut => sut.UpdateActionPlan(updateActionPlan))
                    .Should().ThrowAsync<DssException>();

            }


            [Test]
            public async Task When_UpdateActionErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                restClient.PatchAsync<ActionPlan>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>())
                    .ThrowsForAnyArgs<Exception>();

                _ = await _dssWriter.Invoking(sut => sut.UpdateActionPlan(updateActionPlan))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }

        }

        public class UpdateGoal : DssTests
        {
            private IDssWriter _dssWriter;
            private Models.UpdateGoal updateGoal;

            [SetUp]
            public void Init()
            {

                Logger = Substitute.For<ILogger<DssService>>();
                var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulUpdateGoal(),
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
                    ActionsApiUrl = "https://this.is.anApi.org.uk",
                    ActionsApiVersion = "v3",
                    InteractionsApiUrl = "https://this.is.anApi.org.uk",
                    AdviserDetailsApiVersion = "v2",
                    ActionPlansApiUrl = "https://this.is.anApi.org.uk",
                    ActionPlansApiVersion = "v2",
                    AdviserDetailsApiUrl = "https://this.is.anApi.org.uk",
                    TouchpointId = "9000000001"
                });
                _dssWriter = new DssService(RestClient, DssSettings, Logger);
                updateGoal = new Models.UpdateGoal()
                {
                    CustomerId = new Guid(),
                    InteractionId = new Guid(),
                    ActionPlanId = new Guid(),
                    DateGoalShouldBeCompletedBy = DateTime.Now.AddDays(1),
                    GoalStatus = GoalStatus.Achieved,
                    GoalId = new Guid()
                };
            }


            [Test]
            public Task When_UpdateGoal_ReturnActionPlan()
            {
                return _dssWriter.UpdateGoal(updateGoal);
            }

            [Test]
            public async Task When_UpdateGoalNoSuccess_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                _ = await _dssWriter.Invoking(sut => sut.UpdateGoal(updateGoal))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_UpdateGoalErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                restClient.PatchAsync<Goal>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>())
                    .ThrowsForAnyArgs<Exception>();

                _ = await _dssWriter.Invoking(sut => sut.UpdateGoal(updateGoal))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }

        }

        public class UpdateAction : DssTests
        {
            private IDssWriter _dssWriter;
            private Models.UpdateAction updateAction;

            [SetUp]
            public void Init()
            {

                Logger = Substitute.For<ILogger<DssService>>();
                var mockHandler = DssHelpers.GetMockMessageHandler(DssHelpers.SuccessfulUpdateGoal(),
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
                    ActionsApiUrl = "https://this.is.anApi.org.uk",
                    ActionsApiVersion = "v3",
                    InteractionsApiUrl = "https://this.is.anApi.org.uk",
                    AdviserDetailsApiVersion = "v2",
                    ActionPlansApiUrl = "https://this.is.anApi.org.uk",
                    ActionPlansApiVersion = "v2",
                    AdviserDetailsApiUrl = "https://this.is.anApi.org.uk",
                    TouchpointId = "9000000001"
                });
                _dssWriter = new DssService(RestClient, DssSettings, Logger);
                updateAction = new Models.UpdateAction()
                {
                    CustomerId = new Guid(),
                    InteractionId = new Guid(),
                    ActionPlanId = new Guid(),
                    DateActionAimsToBeCompletedBy = DateTime.Now.AddDays(1),
                    ActionStatus = ActionStatus.InProgress,
                    ActionId = new Guid()
                };
            }


            [Test]
            public Task When_UpdateGoal_ReturnActionPlan()
            {
                return _dssWriter.UpdateAction(updateAction);
            }

            [Test]
            public async Task When_UpdateGoalNoSuccess_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                _ = await _dssWriter.Invoking(sut => sut.UpdateAction(updateAction))
                    .Should().ThrowAsync<DssException>();
            }

            [Test]
            public async Task When_UpdateGoalErrors_Throw_Exception()
            {
                var restClient = Substitute.For<IRestClient>();
                restClient.LastResponse = new RestClient.APIResponse(new HttpResponseMessage(HttpStatusCode.NoContent))
                {
                    IsSuccess = false
                };
                _dssWriter = new DssService(restClient, DssSettings, Logger);

                restClient.PatchAsync<Goal>(Arg.Any<string>(), Arg.Any<HttpRequestMessage>())
                    .ThrowsForAnyArgs<Exception>();

                _ = await _dssWriter.Invoking(sut => sut.UpdateAction(updateAction))
                    .Should().ThrowAsync<DssException>();
                Logger.ReceivedCalls().Count().Equals(1);
            }

        }
    }
}