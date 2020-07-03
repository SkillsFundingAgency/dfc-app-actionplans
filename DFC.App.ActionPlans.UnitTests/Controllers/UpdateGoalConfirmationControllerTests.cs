using System;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Exceptions;
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
           
        }

        [Test]
        [TestCase(Constants.Constants.Goal,Constants.Constants.Date)]
        [TestCase(Constants.Constants.Goal,Constants.Constants.Status)]
        [TestCase(Constants.Constants.Action,Constants.Constants.Date)]
        [TestCase(Constants.Constants.Action,Constants.Constants.Status)]
        public async Task WhenBodyCalledWithParameters_ReturnHtml(int objectUpdated, int propertyUpdated)
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
                PropertyUpdated = 0,
                UpdateMessage = "Some Message",
                ObjLink = "some link",
                ObjLinkText = "some text",
                WhatChanged = "something"
            };
            var objUpdated = vm.ObjectUpdated;
            var whatChanged = vm.WhatChanged;
            var ObjLink = vm.ObjLink;
            var ObjLinkText = vm.ObjLinkText;
            var WhatChanged = vm.WhatChanged;
            var result = await _controller.Body(new Guid(), new Guid(),new Guid(),objectUpdated,propertyUpdated) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public async Task WhenBodyCalledWithInvalidObjectUpdated_ExceptoinThrown()
        {
            
             _controller.Invoking(sut => sut.Body(new Guid(), new Guid(),new Guid(),0,Constants.Constants.Date))
                .Should().Throw<ObjectUpdatedNotSet>();
        }

        [Test]
        public async Task WhenBodyCalledWithInvalidPropertyUpdated_ExceptoinThrown()
        {
            
            _controller.Invoking(sut => sut.Body(new Guid(), new Guid(),new Guid(),Constants.Constants.Goal,0))
                .Should().Throw<PropertyUpdatedNotSet>();
        }
    }
}
