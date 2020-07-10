using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class ChangeActionStatusControllerTests : BaseControllerTests
    {
        private ChangeActionStatusController _controller;

        [SetUp]
        public void Init()
        {
            _controller = new ChangeActionStatusController(_logger, _compositeSettings, _dssReader,_dssWriter,_cosmosService);
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
