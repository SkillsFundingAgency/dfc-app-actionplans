using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.ViewModels;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    internal class UpdateGoalConfirmationControllerTests : BaseControllerTests
    {
        private UpdateConfirmationController _controller;
        private ILogger<UpdateConfirmationController> _logger;
        private IDocumentService<CmsApiSharedContentModel> _documentService;
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
            _documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            _logger = new Logger<UpdateConfirmationController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<UpdateConfirmationController>>();
            _controller = new UpdateConfirmationController(_logger, _compositeSettings, _dssReader, _cosmosService, _documentService, _config);
            var context = new DefaultHttpContext() { User = user };
            _controller.ControllerContext.HttpContext = context;
            context.Request.Headers["x-dfc-composite-sessionid"] = Guid.NewGuid().ToString();
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
                Interaction = null,
                Adviser = null,
                ContactDetails = null,
                ObjectUpdated = Constants.Constants.Goal,
                PropertyUpdated = 0
            };
            var objUpdated = vm.ObjectUpdated;

        }

        [Test]
        [TestCase(Constants.Constants.Goal, Constants.Constants.Date)]
        [TestCase(Constants.Constants.Goal, Constants.Constants.Status)]
        [TestCase(Constants.Constants.Action, Constants.Constants.Date)]
        [TestCase(Constants.Constants.Action, Constants.Constants.Status)]
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
            var result = await _controller.Body(new Guid(), objectUpdated, propertyUpdated) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

        [Test]
        public void WhenBodyCalledWithInvalidObjectUpdated_ExceptionThrown()
        {

            _controller.Invoking(async sut => await sut.Body(new Guid(), 0, Constants.Constants.Date))
              .Should().ThrowAsync<ObjectUpdatedNotSetException>();
        }

        [Test]
        public void WhenBodyCalledWithInvalidPropertyUpdated_ExceptionThrown()
        {

            _controller.Invoking(async sut => await sut.Body(new Guid(), Constants.Constants.Goal, 0))
                .Should().ThrowAsync<PropertyUpdatedNotSetException>();
        }

        [Test]
        public void WhenHeadCalledWithInvalidObjectUpdated_ExceptionThrown()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"objectUpdated", "ggg"}
            });

            var result =_controller.Head();
            var badRequest = result.As<BadRequestObjectResult>();
            Assert.NotNull(badRequest);
        }

        [Test]
        public void WhenHeadCalledWithvalidObjectUpdated_ReturnView()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.HttpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>()
            {
                {"objectUpdated", "1"}
            });

            var result = _controller.Head() as ViewResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }
    }
}
