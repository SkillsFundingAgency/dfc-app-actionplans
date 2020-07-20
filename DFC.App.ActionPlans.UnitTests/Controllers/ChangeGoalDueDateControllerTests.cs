using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ChangeGoalDueDateControllerTests : BaseControllerTests
    {
         private ChangeGoalDueDateController _controller;
         private ILogger<ChangeGoalDueDateController> _logger;
        [SetUp]
        public void Init()
        {
        
        _logger = new Logger<ChangeGoalDueDateController>(new LoggerFactory());
        _logger = Substitute.For<ILogger<ChangeGoalDueDateController>>();
            _controller = new ChangeGoalDueDateController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
           
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
            var result = await _controller.Body(new Guid(), new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
        
       
       
        [Test]
        public async Task WhenBodyCalledWithFormDataAndGoalUpdated_ThenRedirectToBody()
        {
            var result = await _controller.Body(GetViewModel(), new FormCollection(new Dictionary<string, StringValues>
            {
                {"Day", "1"},
                {"Month", "3"},
                {"Year", "2030"}
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
        private ChangeGoalCompositeViewModel GetViewModel()
        {
            var changeGoalDueDateCompositeViewModel = new ChangeGoalCompositeViewModel()
                {
                    Goal = new Goal(){GoalId = new Guid().ToString()}
                    };
            
            return changeGoalDueDateCompositeViewModel;
        }
    }
}

