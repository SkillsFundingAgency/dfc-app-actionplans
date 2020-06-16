using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dfc.App.ActionPlans.Controllers
{
    public class HomeController : CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        
        public HomeController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,  IOptions<AuthSettings> authSettings, IDssReader dssReader)
            :base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }
        //  [Authorize]
        [Route("/body/{controller}/{actionPlanId}/{interactionId}")]
        [Route("/body/{actionPlanId}/{interactionId}")]

        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId)
        {
            var customer = await GetCustomerDetails();
            List<Session> sessions;
            ViewModel.interaction = await _dssReader.GetInteractionDetails(customer.CustomerId.ToString(), interactionId.ToString());
            ViewModel.adviser = await _dssReader.GetAdviserDetails(ViewModel.interaction.AdviserDetailsId);
            sessions = await _dssReader.GetSessions(customer.CustomerId.ToString(), interactionId.ToString());
            ViewModel.latestSession = sessions.OrderByDescending(s => s.DateandTimeOfSession).First();
            ViewModel.goals = await _dssReader.GetGoals(customer.CustomerId.ToString(), interactionId.ToString(), actionPlanId.ToString());
            ViewModel.actions = await _dssReader.GetActions(customer.CustomerId.ToString(), interactionId.ToString(), actionPlanId.ToString());
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
    }
}
