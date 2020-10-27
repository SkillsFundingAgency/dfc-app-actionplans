using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

namespace Dfc.App.ActionPlans.Controllers
{
   
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        private readonly IDssWriter _dssWriter;
        private readonly IOptions<AuthSettings> _authSettings;

        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService, IOptions<AuthSettings> authSettings)
            :base(compositeSettings, dssReader, cosmosServiceService)
        {
            _dssReader = dssReader;
            _dssWriter = dssWriter;
            _authSettings = authSettings;
        }
        [Authorize]
        [Route("/body/home")]
        [HttpPost]
        public async Task<IActionResult> Body(HomeCompositeViewModel viewModel, IFormCollection formCollection)
        {
            var x = User;

            if (formCollection.FirstOrDefault(x =>
                string.Compare(x.Key, "homeGovukCheckBoxAcceptplan", StringComparison.CurrentCultureIgnoreCase) ==
                0).Value == "on")
            {
                await _dssWriter.UpdateActionPlan(new UpdateActionPlan()
                {
                  CustomerId  = viewModel.CustomerId,
                  InteractionId = viewModel.InteractionId,
                  ActionPlanId = viewModel.ActionPlanId,
                  DateActionPlanAcknowledged = DateTime.UtcNow.AddMinutes(-1)
                });
            }
            var customer = await GetCustomerDetails();
            await  LoadData(customer.CustomerId,viewModel.ActionPlanId,viewModel.InteractionId);
            ViewModel.LatestSession = await GetLatestSession();
            return RedirectTo($"{viewModel.ActionPlanId}/{viewModel.InteractionId}");
        }

        [Route("/body")]
        [HttpGet]
        public override async Task<IActionResult> Body()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(_authSettings.Value.AccountEndpoint);
            }

            return await Task.FromResult<IActionResult>(View("BodyUnAuth", ViewModel));
        }

        [Authorize]
        [Route("/body/{actionPlanId}/{interactionId}")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId)
        {
            var customer = await GetCustomerDetails();
            await  LoadData(customer.CustomerId,actionPlanId,interactionId);
            ViewModel.Goals = await _dssReader.GetGoals(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.Actions = await _dssReader.GetActions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.ActionPlan = await _dssReader.GetActionPlanDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.LatestSession = await GetLatestSession();
            return await base.Body();
        }

        #region Default Routes
        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/home/{actionPlanId?}/{interactionId?}/{objId?}/{objupdated?}/{propertyUpdated?}")]
        [Route("/head/{actionPlanId?}/{interactionId?}/{objId?}/{objupdated?}/{propertyUpdated?}")]
        public override IActionResult Head(Guid actionPlanId, Guid interactionId, Guid objId, int objectUpdated, int propertyUpdated)
        {
            return base.Head(actionPlanId, interactionId, objId, objectUpdated, propertyUpdated);
        }
        [Route("/bodytop/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/bodytop/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public override async Task<IActionResult> BodyTop()
        {
            return await base.BodyTop();
        }
        [Route("/breadcrumb/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/breadcrumb/{id?}/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public override IActionResult Breadcrumb(Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            return base.Breadcrumb(actionPlanId, interactionId, objectId);
        }
      

        [Route("/bodyfooter/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/bodyfooter/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public override IActionResult BodyFooter()
        {
            return base.BodyFooter();
        }
        #endregion Default Routes

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<Session> GetLatestSession()
        {
            List<Session> sessions = await _dssReader.GetSessions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            return sessions.OrderByDescending(s => s.DateandTimeOfSession).First();
        }
    }
}
