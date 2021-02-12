using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class SessionControllerTests : BaseControllerTests
    {
        private HomeController _controller;
        private ILogger<HomeController> _logger;
        private IDocumentService<CmsApiSharedContentModel> _documentService;
        private IConfiguration _config;

        [SetUp]
        public void Init()
        {
            _logger = new Logger<HomeController>(new LoggerFactory());
            _logger = Substitute.For<ILogger<HomeController>>();
            _cosmosService= Substitute.For<ICosmosService>();
            var inMemorySettings = new Dictionary<string, string> {
                {DFC.APP.ActionPlans.Data.Common.Constants.SharedContentGuidConfig, Guid.NewGuid().ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
                .Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = null
            });
            _documentService = Substitute.For<IDocumentService<CmsApiSharedContentModel>>();
            _controller = new HomeController(_logger, _compositeSettings, _dssReader, _dssWriter, _cosmosService, Options.Create(new AuthSettings { AccountEndpoint = "https://www.g.com" }), _documentService, _config);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext(){User = user};
            
           

        }

        [Test]
        public async Task When_HomeCalledWithValues_Then_NewSessionCreated()
        {
            var actionId = Guid.NewGuid();
            var interactionId = Guid.NewGuid();
            
            var result = await _controller.Body(actionId, interactionId) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            await _cosmosService.Received().CreateItemAsync(Arg.Any<UserSession>(), Arg.Any<CosmosCollection>());
        }

        [Test]
        public async Task When_SessionExist_Then_UpdateSession()
        {
            var actionId = Guid.NewGuid();
            var interactionId = Guid.NewGuid();

            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
                .ReturnsForAnyArgs(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new UserSession
                    {
                        InteractionId = Guid.NewGuid(),
                        ActionPlanId = Guid.NewGuid(),
                        CustomerId = new Guid("c2e27821-cc60-4d3d-b4f0-cbe20867897c"),
                        Adviser = new Adviser(),
                        Id = "",
                        Interaction = null
                    }))

                });

            var result = await _controller.Body(actionId, interactionId) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
            await _cosmosService.Received().UpsertItemAsync(Arg.Any<UserSession>(), Arg.Any<CosmosCollection>());
        }

    }
}
