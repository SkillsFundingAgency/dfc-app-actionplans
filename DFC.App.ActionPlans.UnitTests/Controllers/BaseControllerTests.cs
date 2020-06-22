using System;
using System.Collections.Generic;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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


        [SetUp]
        public void SetupBase()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = Substitute.For<ILogger<HomeController>>();
            _dssReader = Substitute.For<IDssReader>();
            _dssWriter = Substitute.For<IDssWriter>();
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
                AdviserDetailId = null,
                AdviserName = null,
                AdviserEmailAddress = null,
                AdviserContactNumber = null,
                LastModifiedDate = default,
                LastModifiedTouchpointId = null,
                SubcontractorId = null
            };
            var actionPlan = new ActionPlan
            {
                ActionPlanId = null,
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
                GoalId = null,
                CustomerId = null,
                ActionPlanId = null,
                SubcontractorId = null,
                DateGoalCaptured = default,
                DateGoalShouldBeCompletedBy = default,
                DateGoalAchieved = default,
                GoalSummary = null,
                GoalType = GoalType.Skills,
                GoalStatus = 0,
                LastModifiedDate = default,
                LastModifiedBy = null
            };
            var action = new Services.DSS.Models.Action
            {
                ActionId = null,
                CustomerId = null,
                ActionPlanId = null,
                DateActionAgreed = default,
                DateActionAimsToBeCompletedBy = default,
                DateActionActuallyCompleted = default,
                ActionSummary = null,
                SignpostedTo = null,
                SignpostedToCategory = null,
                ActionType = (ActionType) 0,
                ActionStatus = (ActionStatus) 0,
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

        }


    }
}
