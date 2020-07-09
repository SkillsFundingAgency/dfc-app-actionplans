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
    [Authorize]
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        private readonly IDssWriter _dssWriter;

        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService)
            :base(compositeSettings, dssReader, cosmosServiceService)
        {
            _dssReader = dssReader;
            _dssWriter = dssWriter;
        }
        
        [Route("/body/home")]
        [HttpPost]
        public async Task<IActionResult> Body(HomeCompositeViewModel viewModel, IFormCollection formCollection)
        {
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
            ViewModel.LatestSession = await GetLatestSession();
            return RedirectTo($"{CompositeViewModel.PageId.Home.Value}/{viewModel.ActionPlanId}/{viewModel.InteractionId}");
        }

        //  [Authorize]
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
        [Route("/head/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/head/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Route("/bodytop/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/bodytop/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public override async Task<IActionResult> BodyTop()
        {
            return await base.BodyTop();
        }
        [Route("/breadcrumb/home/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        [Route("/breadcrumb/{id}/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
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
