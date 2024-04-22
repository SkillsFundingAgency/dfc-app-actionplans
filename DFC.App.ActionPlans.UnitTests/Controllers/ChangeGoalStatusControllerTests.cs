using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
//using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ChangeGoalStatusControllerTests : BaseControllerTests
    {
        private ChangeGoalStatusController _controller;
        private ILogger<ChangeGoalStatusController> _logger;
        private ISharedContentRedisInterface _sharedContentRedisInterface;
        private IConfiguration _config;
        [SetUp]
        public void Init()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {DFC.APP.ActionPlans.Data.Common.Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _sharedContentRedisInterface = Substitute.For<ISharedContentRedisInterface>();
            _logger = new Logger<ChangeGoalStatusController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<ChangeGoalStatusController>>();
            _controller = new ChangeGoalStatusController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService, _sharedContentRedisInterface, _config);
            var context = new DefaultHttpContext() { User = user };
            _controller.ControllerContext.HttpContext = context;
            context.Request.Headers["x-dfc-composite-sessionid"] = Guid.NewGuid().ToString();
            var routeData = new RouteData();
            routeData.Values.Add("controller", Constants.Constants.ChangeGoalStatusController);
            _controller.ControllerContext.RouteData = routeData;
        }

        [Test]
        public async Task WhenBreadcrumbCalledWithGuid_ReturnHtml()
        {
            var id = Guid.NewGuid();
            var result = _controller.Breadcrumb(id) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            var data = result.ViewData.Model as ChangeGoalCompositeViewModel;
            data.BackLink.Should().Contain(id.ToString());
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var result = await _controller.Body(new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        
        [Test]
        public async Task WhenBodyCalledWithFormDataAndGoalUpdated_ThenRedirectToBody()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"GoalStatus", "1"}
            })) as RedirectResult;

            result.Url.Should().Contain("update-confirmation");
        }
        
        [Test]
        public async Task WhenBodyCalledWithNoGoalStatus_ThenReturnToBodyWithError()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"GoalStatus", ""}
            })) as ViewResult;;

          
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as ChangeGoalCompositeViewModel;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
            
        }

        private ChangeGoalCompositeViewModel GetViewModel()
        {
            var changeGoalDueDateCompositeViewModel = new ChangeGoalCompositeViewModel()
            {
                    Goal = new Goal()
                    {
                        GoalId = new Guid().ToString(),
                        GoalStatus = GoalStatus.Achieved,
                        DateGoalShouldBeCompletedBy = DateTime.Now.AddDays(1)
                    }
            };
            
            return changeGoalDueDateCompositeViewModel;
        }
    }
}
