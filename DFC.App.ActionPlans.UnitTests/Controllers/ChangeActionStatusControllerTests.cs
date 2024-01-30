using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
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
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ChangeActionStatusControllerTests : BaseControllerTests
    {
        private ChangeActionStatusController _controller;
        private ILogger<ChangeActionStatusController> _logger;
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
            _logger = new Logger<ChangeActionStatusController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<ChangeActionStatusController>>();
            _controller = new ChangeActionStatusController(_logger, _compositeSettings, _dssReader,_dssWriter,_cosmosService, _sharedContentRedisInterface, _config);
            var context = new DefaultHttpContext() { User = user };
            _controller.ControllerContext.HttpContext = context;
            context.Request.Headers["x-dfc-composite-sessionid"] = Guid.NewGuid().ToString();
            var routeData = new RouteData();
            routeData.Values.Add("controller", Constants.Constants.ChangeActionStatusController);
            _controller.ControllerContext.RouteData = routeData;
        }
        
        [Test]
        public async Task WhenBreadcrumbCalledWithGuid_ReturnHtml()
        {
            var id = Guid.NewGuid();
            var result = _controller.Breadcrumb(id) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            var data = result.ViewData.Model as ChangeActionCompositeViewModel;
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
                {"ActionStatus", "1"}
            })) as RedirectResult;

            result.Url.Should().Contain("update-confirmation");
        }
        
        [Test]
        public async Task WhenBodyCalledWithBlankDateAndGoalUpdated_ThenReturnToBodyWithError()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"Day", ""},
                {"Month", ""},
                {"Year", ""}
            })) as ViewResult;;

          
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as ChangeGoalCompositeViewModel;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
            
        }

        [Test]
        public async Task WhenBodyCalledWithInvalidDateAndGoalUpdated_ThenReturnToBodyWithError()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"Day", "1"},
                {"Month", "31"},
                {"Year", "2000"}
            })) as ViewResult;;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as ChangeGoalCompositeViewModel;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }

        [Test]
        public async Task WhenBodyCalledWithHisotricDateAndGoalUpdated_ThenReturnToBodyWithError()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"Day", "1"},
                {"Month", "12"},
                {"Year", "2000"}
            })) as ViewResult;;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            var model = result.ViewData.Model as ChangeGoalCompositeViewModel;
            result.ViewData.ModelState.IsValid.Should().BeFalse();
        }
        private ChangeActionCompositeViewModel GetViewModel()
        {
            var changeActionDueDateCompositeViewModel = new ChangeActionCompositeViewModel()
                {
                    Action = new Action()
                    {
                        ActionId = new Guid().ToString(),
                        ActionStatus = ActionStatus.Completed,
                        DateActionAimsToBeCompletedBy = DateTime.Now.AddDays(1)
                    }
                };
            
            return changeActionDueDateCompositeViewModel;
        }
    }
}
