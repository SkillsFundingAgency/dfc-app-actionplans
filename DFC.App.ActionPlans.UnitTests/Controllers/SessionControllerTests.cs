using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.App.ActionPlans.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class SessionControllerTests : BaseControllerTests
    {
        private ChangeActionDueDateController _controller;
        private ICosmosService _cosmosService;
        private ILogger<ChangeActionDueDateController> _logger;

        [SetUp]
        public void Init()
        {
            _logger = new Logger<ChangeActionDueDateController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<ChangeActionDueDateController>>();
            _cosmosService= Substitute.For<ICosmosService>();
            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
            .Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = null
            });
            _controller = new ChangeActionDueDateController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
           
        }

        [Test]
        public async Task When_CreateUserSession_Then_NewSessionCreated()
        {
            var request = new CreateSessionRequest
            {
                ActionPlanId = default
            };
            var result = await _controller.Body(new Guid(), new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

    }
}
