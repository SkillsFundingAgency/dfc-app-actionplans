using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
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
        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
            :base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
            _dssWriter = dssWriter;
        }
        [Route("/body/{controller}")]
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
            return RedirectTo($"{CompositeViewModel.PageId.Home.Value}/{viewModel.ActionPlanId}/{viewModel.InteractionId}");
        }

        //  [Authorize]
        [Route("/body/{controller}/{actionPlanId}/{interactionId}")]
        [Route("/body/{actionPlanId}/{interactionId}")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId)
        {
            var customer = await GetCustomerDetails();
            await  LoadData(customer.CustomerId,actionPlanId,interactionId);
            return await base.Body();
        }

        #region Default Routes
        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/{controller}/{id}/{intid}")]
        [Route("/head/{id}/{intid}")]
        public override IActionResult Head()
        {
            return base.Head();
        }
        [Route("/bodytop/{controller}/{id}/{intid}")]
        [Route("/bodytop/{id}/{intid}")]
        public override async Task<IActionResult> BodyTop()
        {
            return await base.BodyTop();
        }
        [Route("/breadcrumb/{controller}/{id}/{intid}")]
        [Route("/breadcrumb/{id}/{intid}")]
        public override IActionResult Breadcrumb()
        {
            return base.Breadcrumb();
        }
      

        [Route("/bodyfooter/{controller}/{id}/{intid}")]
        [Route("/bodyfooter/{id}/{intid}")]
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

        private async Task LoadData(Guid customerId, Guid actionPlanId, Guid interactionId)
        {
            List<Session> sessions;
            ViewModel.CustomerId = customerId;
            ViewModel.InteractionId = interactionId;
            ViewModel.ActionPlanId = actionPlanId;
            ViewModel.Interaction = await _dssReader.GetInteractionDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            ViewModel.Adviser = await _dssReader.GetAdviserDetails(ViewModel.Interaction.AdviserDetailsId);
            sessions = await _dssReader.GetSessions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            ViewModel.LatestSession = sessions.OrderByDescending(s => s.DateandTimeOfSession).First();
            ViewModel.Goals = await _dssReader.GetGoals(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.Actions = await _dssReader.GetActions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
            ViewModel.ActionPlan = await _dssReader.GetActionPlan(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString());
        }

    }
}
