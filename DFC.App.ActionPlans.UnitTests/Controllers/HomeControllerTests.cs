using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<HomeController> _logger;
        private IOptions<AuthSettings> _authSettings;
        private IDssReader _dssReader;
        private IDssWriter _dssWriter;
        private HomeController _controller;

        [SetUp]
        public void Init()
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
                AdviserDetailsId =  new Guid().ToString(),
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
            var action = new Action
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
            _dssReader.GetInteractionDetails(Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(interaction);
            _dssReader.GetActionPlan(Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(actionPlan);
            _dssReader.GetSessions(Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(new List<Session> {session});
            _dssReader.GetGoals(Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(new List<Goal> {goal});
            _dssReader.GetActions(Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(new List<Action> {action});
            _dssReader.GetActionPlan(Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>()).ReturnsForAnyArgs(actionPlan);
            _dssWriter.UpdateActionPlan(Arg.Any<UpdateActionPlan>());
            _controller = new HomeController(_logger, _compositeSettings,_authSettings, _dssReader,_dssWriter);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
           
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var result = _controller.Head() as ViewResult;
            var vm = new HeadViewModel {PageTitle = "Page Title",};
            var pageTitle = vm.PageTitle;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var result = await _controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        [Test]
        public async Task WhenBodyCalledWithParameters_ReturnHtml()
        {
            var result = await _controller.Body(new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyTopCalled_ReturnHtml()
        {
            var result = await _controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyTopCalled_ReturnHtmlWithUserName()
        {
          
        
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    HttpContext =
                    {
                        User = new ClaimsPrincipal(
                            new ClaimsIdentity(new List<Claim> {new Claim("CustomerId", "test")},
                                "testType"))
                    }
                }
            };
            var result = await _controller.BodyTop() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as CompositeViewModel;
            model.Name.Should().NotBe(null);
        }

        [Test]
        public void WhenBodyFooterCalled_ReturnHtml()
        {
            var result = _controller.BodyFooter() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenErrorCalled_ReturnHtml()
        {
            var result = _controller.Error() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalledWithFormDataAndActionPlanUpdated_ThenRedirectToBody()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"homeGovukCheckBoxAcceptplan", "on"}
            })) as RedirectResult;

            result.Url.Should().Contain("~/home");
        }
        

        private HomeCompositeViewModel GetViewModel()
        {
            var homeCompositeViewModel = new HomeCompositeViewModel();
            return homeCompositeViewModel;
        }
    }
}


