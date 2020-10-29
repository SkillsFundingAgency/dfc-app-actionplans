﻿using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private IOptions<CompositeSettings> _compositeSettings;
        private ILogger<ErrorController> _logger;
        protected readonly IDssReader _dssReader;
        protected readonly ICosmosService _cosmosService;
        private IOptions<CompositeSettings> _options;


        [SetUp]
        public void Init()
        {
            _logger = Substitute.For<ILogger<ErrorController>>();
            _compositeSettings = Options.Create(new CompositeSettings());
            _logger = new Logger<ErrorController>(new LoggerFactory());
            _options = Options.Create(new CompositeSettings());
        }

        [Test]
        public async Task WhenBodyCalled_ReturnHtml()
        {
            var controller = new ErrorController(_logger, _compositeSettings, _dssReader, _cosmosService, _options);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Body() as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

    }
}
