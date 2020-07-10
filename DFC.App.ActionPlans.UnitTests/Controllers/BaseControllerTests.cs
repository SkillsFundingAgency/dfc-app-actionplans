using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    public abstract class BaseControllerTests
    {
        protected IOptions<CompositeSettings> _compositeSettings;
        protected ILogger<HomeController> _logger;
        protected IOptions<AuthSettings> _authSettings;
        protected IDssReader _dssReader;
        protected IDssWriter _dssWriter;
        protected ICosmosService _cosmosService;
        protected ClaimsPrincipal user;

        [SetUp]
        public void SetupBase()
        {
            user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("CustomerId", new Guid().ToString()),
            }, "mock"));

           
           
            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings
            {
                Cdn = "cdn",
                Path = "Path"

            });
            _logger = Substitute.For<ILogger<HomeController>>();
            _dssReader = Substitute.For<IDssReader>();
            _dssWriter = Substitute.For<IDssWriter>();
            _cosmosService= Substitute.For<ICosmosService>();
            _authSettings = Options.Create(new AuthSettings
            {
                RegisterUrl = "reg", SignInUrl = "signin", SignOutUrl = "signout"
            });

            var customer = new Customer
            {
                CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                FamilyName = "familyName",
                GivenName = "givenName"
            };
            var adviser = new Adviser
            {
                AdviserDetailId = new Guid().ToString(),
                AdviserName = null,
                AdviserEmailAddress = null,
                AdviserContactNumber = null,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null,
                SubcontractorId = null
            };
            var actionPlan = new ActionPlan
            {
                ActionPlanId = new Guid().ToString(),
                CustomerId = null,
                InteractionId = null,
                SessionId = null,
                SubcontractorId = null,
                DateActionPlanCreated = default,
                CustomerCharterShownToCustomer = null,
                DateAndTimeCharterShown = default,
                DateActionPlanSentToCustomer = default,
                ActionPlanDeliveryMethod = null,
                DateActionPlanAcknowledged = default,
                CurrentSituation = null,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null
            };
            var interaction = new Interaction
            {
                InteractionId = new Guid().ToString(),
                CustomerId = null,
                TouchpointId = null,
                AdviserDetailsId = new Guid().ToString(),
                DateandTimeOfInteraction = default,
                Channel = null,
                InteractionType = null,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null
            };
            var session = new Session
            {
                SessionId = new Guid().ToString(),
                CustomerId = null,
                InteractionId = null,
                DateandTimeOfSession = default,
                VenuePostCode = null,
                SessionAttended = null,
                ReasonForNonAttendance = null,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null,
                SubcontractorId = null
            };
            var goal = new Goal
            {
                GoalId = new Guid().ToString(),
                CustomerId = null,
                ActionPlanId = null,
                SubcontractorId = null,
                DateGoalCaptured = default,
                DateGoalShouldBeCompletedBy = default,
                DateGoalAchieved = default,
                GoalSummary = null,
                GoalType = GoalType.Skills,
                GoalStatus = GoalStatus.Achieved,
                LastModifiedDate = default,
                LastModifiedBy = null
            };
            var action = new Services.DSS.Models.Action
            {
                ActionId = new Guid().ToString(),
                CustomerId = null,
                ActionPlanId = null,
                DateActionAgreed = default,
                DateActionAimsToBeCompletedBy = default,
                DateActionActuallyCompleted = default,
                ActionSummary = null,
                SignpostedTo = null,
                SignpostedToCategory = null,
                ActionType = ActionType.ApplyForApprenticeship,
                ActionStatus = ActionStatus.InProgress,
                PersonResponsible = 0,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null
            };

            _dssReader.GetCustomerDetails(Arg.Any<string>()).ReturnsForAnyArgs(customer);
            _dssReader.GetAdviserDetails(Arg.Any<string>()).ReturnsForAnyArgs(adviser);
            _dssReader.GetInteractionDetails(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(interaction);
            _dssReader.GetActionPlanDetails(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(actionPlan);
            _dssReader.GetSessions(Arg.Any<string>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Session> {session});
            _dssReader.GetGoals(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(new List<Goal> {goal});
            _dssReader.GetActions(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(new List<Services.DSS.Models.Action> {action});
            _dssWriter.UpdateActionPlan(Arg.Any<UpdateActionPlan>());
            _dssReader.GetGoalDetails(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>())
                .ReturnsForAnyArgs(goal);
            _dssReader.GetActionDetails(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>())
                .ReturnsForAnyArgs(action);
            
            var userSession =  new UserSession
            {
                Id = default,
                CustomerId = default,
                InteractionId = default,
                ActionPlanId = default,
                Interaction = null,
                Adviser = null
            };
            var userSessionJson = new StringContent(JsonConvert.SerializeObject(userSession));
            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = userSessionJson
                });
        }


    }
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
        {
        }
    }

    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims)
        {
        }
    }
}
