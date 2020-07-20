using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Services.DSS.Enums;
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
    class ChangeGoalStatusControllerTests : BaseControllerTests
    {
        private ChangeGoalStatusController _controller;
        private ILogger<ChangeGoalStatusController> _logger;
        [SetUp]
        public void Init()
        {
            _logger = new Logger<ChangeGoalStatusController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<ChangeGoalStatusController>>();
            _controller = new ChangeGoalStatusController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
           
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
