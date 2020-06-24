using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ViewActionControllerTests : BaseControllerTests
    {
        private ViewActionController _controller;

        [SetUp]
        public void Init()
        {
            _controller = new ViewActionController(_logger, _compositeSettings, _dssReader);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

       
        
        [Test]
        public async Task WhenBodyCalledWithParameters_ReturnHtml()
        {
            var result = await _controller.Body(new Guid(), new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

       

       

   
    }
}
