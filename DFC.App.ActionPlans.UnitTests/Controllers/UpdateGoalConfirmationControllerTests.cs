using System;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.Services.DSS.Services;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
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
            var vm = new UpdateGoalConfirmationCompositeViewModel
            {
                PageTitle = null,
                PageHeading = null,
                Name = null,
                ShowBreadCrumb = false,
                CompositeSettings = null,
                CustomerId = default,
                ActionPlanId = default,
                InteractionId = default,
                LatestSession = null,
                Interaction = null,
                Adviser = null,
                ContactDetails = null,
                ObjectUpdated = Constants.Constants.Goal,
                PropertyUpdated = 0
            };
            var objUpdated = vm.ObjectUpdated;

            var result = await _controller.Body(new Guid(), new Guid(),new Guid(),Constants.Constants.Goal,Constants.Constants.Date) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
