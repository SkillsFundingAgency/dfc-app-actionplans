using System;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class UpdateGoalConfirmationControllerTests : BaseControllerTests
    {
        private UpdateGoalConfirmationController _controller;

        [SetUp]
        public void Init()
        {
           
            _controller = new UpdateGoalConfirmationController(_logger, _compositeSettings, _dssReader);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
           
        }

        [Test]
        public async Task WhenBodyCalledWithParameters_ReturnHtml()
        {
            var result = await _controller.Body(new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
