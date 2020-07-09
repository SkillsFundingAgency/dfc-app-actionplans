﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Cosmos.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests.Controllers
{
    class SessionControllerTests : BaseControllerTests
    {
        private ChangeActionDueDateController _controller;
        protected ICosmosService _cosmosService;

        [SetUp]
        public void Init()
        {
            _cosmosService= Substitute.For<ICosmosService>();
            _cosmosService.ReadItemAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CosmosCollection>())
            .Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = null
            });
            _controller = new ChangeActionDueDateController(_logger, _compositeSettings, _dssReader,_dssWriter, _cosmosService);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
           
        }

        [Test]
        public async Task When_CreateUserSession_Then_NewSessionCreated()
        {
            var result = await _controller.Body(new Guid(), new Guid(), new Guid()) as ViewResult;
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();
            result.ViewName.Should().BeNull();
        }

    }
}
