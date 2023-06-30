using System;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace DFC.App.ActionPlans.Controllers
{
    [AllowAnonymous]
    public class StartController : Controller // CompositeSessionController<HomeCompositeViewModel>
    {
        private readonly ILogger _dsslogger;

        public StartController(ILogger<StartController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService)
            //: base(compositeSettings, dssReader, cosmosServiceService)
        {

        }

        [Route("/body/start/")]
        [HttpGet]
        public IActionResult Body(Guid actionPlanId, Guid interactionId, Guid goalId)
        {
            _dsslogger.LogInformation($"StartController Body actionPlanId {actionPlanId} interactionId {interactionId} ");
            return View();
        }


        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/start/")]
        public IActionResult Head(Guid actionPlanId, Guid interactionId, Guid objId, int objectUpdated, int propertyUpdated)
        {
            _dsslogger.LogInformation($"StartController Head actionPlanId {actionPlanId} interactionId {interactionId} ");
            return View();
        }
        [Route("/bodytop/start/")]
        public IActionResult BodyTop()
        {
            _dsslogger.LogInformation($"StartController BodyTop /bodytop/start/");
            return View();
        }
        [Route("/breadcrumb/start/")]
        public IActionResult Breadcrumb(Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            _dsslogger.LogInformation($"StartController Breadcrumb actionPlanId {actionPlanId} interactionId {interactionId} ");
            return View();
        }
      

        [Route("/bodyfooter/start/")]
        public  IActionResult BodyFooter()
        {
            _dsslogger.LogInformation($"StartController BodyFooter /bodyfooter/start/ ");
            return View();
        }
    }

}
