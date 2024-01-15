using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
//using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    public class HomeControllerTests : BaseControllerTests
    {
       
        private HomeController _controller;
        private ILogger<HomeController> _logger;
        private ISharedContentRedisInterface _sharedContentRedisInterface;
        private IConfiguration _config;
        [SetUp]
        public void Init()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<HomeController>>();
            var inMemorySettings = new Dictionary<string, string> {
                {DFC.APP.ActionPlans.Data.Common.Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _sharedContentRedisInterface = Substitute.For<ISharedContentRedisInterface>();
            _controller = new HomeController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService, Options.Create(new AuthSettings{AccountEndpoint = "https://www.g.com"}), _sharedContentRedisInterface, _config);

            var context = new DefaultHttpContext() {User = user};
            _controller.ControllerContext.HttpContext = context;
            context.Request.Headers["x-dfc-composite-sessionid"] = Guid.NewGuid().ToString();
            _controller.ControllerContext.RouteData = new RouteData();
            _controller.ControllerContext.RouteData.Values.Add("controller", Constants.Constants.ChangeGoalDueDateController);


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
        public async Task WhenBodyCalledAndUserNotLoggedIn_ReturnHtml()
        {
            var controller = new HomeController(_logger, _compositeSettings, _dssReader, _dssWriter, _cosmosService,
                Options.Create(new AuthSettings()), _sharedContentRedisInterface, _config)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext() {User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()))}}
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task WhenBodyCalledWithParameters_ReturnHtml()
        {
            var result = await _controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("BodyUnAuth");
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
                .ReturnsForAnyArgs(new HttpResponseMessage(HttpStatusCode.FailedDependency));
               var result = await _controller.Body(Guid.Empty, Guid.Empty) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().Be("BodyUnAuth");
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb(new Guid() ) as ViewResult;
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

        [Test]
        public async Task WhenBodyCalledWithFormDataAndActionPlanUpdated_ThenRedirectToBody()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"homeGovukCheckBoxAcceptplan", "on"}
            })) as RedirectResult;

            result.Url.Should().Contain("~Path/");
        }
        

        private HomeCompositeViewModel GetViewModel()
        {
            var homeCompositeViewModel = new HomeCompositeViewModel();
            return homeCompositeViewModel;
        }
    }
}


