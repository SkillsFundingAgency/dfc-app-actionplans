using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ViewGoalControllerTests : BaseControllerTests
    {
        private ViewGoalController _controller;

        [SetUp]
        public void Init()
        {
            _controller = new ViewGoalController(_logger, _compositeSettings, _dssReader, _cosmosService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
            _controller.ControllerContext.RouteData = new RouteData();
            _controller.ControllerContext.RouteData.Values.Add("controller", Constants.Constants.ChangeGoalDueDateController);
        }

        [Test]
        public void WhenHeadCalled_ReturnHtml()
        {
            var result = _controller.Head(default, default, default, default, default) as ViewResult;
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
            var result = await _controller.Body(new Guid(), new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBreadCrumbCalled_ReturnHtml()
        {
            var result = _controller.Breadcrumb(new Guid(),new Guid(),new Guid()) as ViewResult;
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
