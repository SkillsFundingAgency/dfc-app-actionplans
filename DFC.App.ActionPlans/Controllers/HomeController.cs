using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DFC.APP.ActionPlans.Data.Common;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Dfc.App.ActionPlans.Controllers
{
    [ExcludeFromCodeCoverage]
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        private readonly IDssWriter _dssWriter;
        private readonly IOptions<AuthSettings> _authSettings;
        private readonly ILogger<HomeController> _logger;
        private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;

        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService, IOptions<AuthSettings> authSettings,
            IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            :base(compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssReader = dssReader;
            _dssWriter = dssWriter;
            _authSettings = authSettings;
            _logger = logger;
            _sharedContent = config.GetValue<Guid>(Constants.SharedContentGuidConfig);
            _documentService = documentService;
        }
        [Authorize]
        [Route("/body/home")]
        [HttpPost]
        public async Task<IActionResult> Body(HomeCompositeViewModel viewModel, IFormCollection formCollection)
        {
            var session = await GetUserSession();
            if (formCollection.FirstOrDefault(x =>
                string.Compare(x.Key, "homeGovukCheckBoxAcceptplan", StringComparison.CurrentCultureIgnoreCase) ==
                0).Value == "on")
            {
                await _dssWriter.UpdateActionPlan(new UpdateActionPlan()
                {
                  CustomerId  = session.CustomerId,
                  InteractionId = session.InteractionId,
                  ActionPlanId = session.ActionPlanId,
                  DateActionPlanAcknowledged = DateTime.UtcNow.AddMinutes(-1)
                });
            }

            await  ManageSession(session.CustomerId, session.ActionPlanId, session.InteractionId);
            return RedirectTo("/home");
        }

        [Route("/body")]
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            var sharedContent = await _documentService.GetByIdAsync(_sharedContent, "account").ConfigureAwait(false);
            ViewModel.SharedContent = sharedContent?.Content;
            _logger.LogInformation("Request for Homepage by unauthed user");
            return await Task.FromResult<IActionResult>(View("BodyUnAuth", ViewModel));
        }

        [Authorize]
        [Route("/body/home")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId)
        {
            var session = await GetUserSession();
            
            if (session == null && (actionPlanId == Guid.Empty || interactionId == Guid.Empty))
            {
                return await Task.FromResult<IActionResult>(View("BodyUnAuth", ViewModel));
            }

            _logger.LogInformation("Request for Home/body");
            var timer = new Stopwatch();
            timer.Start();
            var customer = await GetCustomerDetails();
            await ManageSession(customer.CustomerId, actionPlanId, interactionId, session);
            ViewModel.Goals = await _dssReader.GetGoals(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.Actions = await _dssReader.GetActions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.ActionPlan = await _dssReader.GetActionPlanDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.LatestSession = await GetLatestSession();
            timer.Stop();
            _logger.LogInformation($"Body request took {timer.Elapsed}");
            return await base.Body();
        }

        #region Default Routes
        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/home")]
        [Route("/head")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Route("/bodytop/home")]
        [Route("/bodytop")]
        public override async Task<IActionResult> BodyTop()
        {
            if (Request.Query.ContainsKey("actionplanID") || (User.Identity.IsAuthenticated && await GetUserSession() != null))
            {
                ViewModel.HideHeroBanner = true;
            }
            return await base.BodyTop();
        }
        [Route("/breadcrumb/home")]
        [Route("/breadcrumb")]
        public override IActionResult Breadcrumb(Guid objectId)
        {
            return base.Breadcrumb(objectId);
        }
      

        [Route("/bodyfooter/home")]
        [Route("/bodyfooter")]
        public override IActionResult BodyFooter()
        {
            return base.BodyFooter();
        }
        #endregion Default Routes
        
        private async Task<Session> GetLatestSession()
        {
            _logger.LogInformation("Getting sessions from DSS");
            List<Session> sessions = await _dssReader.GetSessions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            _logger.LogInformation("retrieved sessions from DSS");
            return sessions.OrderByDescending(s => s.DateandTimeOfSession).First();
        }
    }
}
