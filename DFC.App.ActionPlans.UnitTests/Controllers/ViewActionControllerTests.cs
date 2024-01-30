using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
//using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ViewActionControllerTests : BaseControllerTests
    {
        private ViewActionController _controller;
        private ILogger<ViewActionController> _logger;
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
            _logger = new Logger<ViewActionController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<ViewActionController>>();
            _controller = new ViewActionController(_logger, _compositeSettings, _dssReader, _cosmosService, _sharedContentRedisInterface, _config);
            var context = new DefaultHttpContext() { User = user };
            _controller.ControllerContext.HttpContext = context;
            context.Request.Headers["x-dfc-composite-sessionid"] = Guid.NewGuid().ToString();
            _controller.ControllerContext.RouteData = new RouteData();
            _controller.ControllerContext.RouteData.Values.Add("controller", Constants.Constants.ChangeGoalDueDateController);
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
            var result = await _controller.Body(new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb(new Guid()) as ViewResult;
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
        public void WhenBodyFooterCalled_ReturnHtml()
        {
            var result = _controller.BodyFooter() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

   
    }
}
