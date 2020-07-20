using System;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    [AllowAnonymous]
    public class StartController : Controller // CompositeSessionController<HomeCompositeViewModel>
    {

        
        public StartController(ILogger<StartController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService)
            //: base(compositeSettings, dssReader, cosmosServiceService)
        {

        }

        [Route("/body/start/")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
        {
            //return await base.Body();
            return View();
        }


        // The home page uses MVC default routes, so we need non "/[controller]" attribute routed versions of the endpoints just for here
        [Route("/head/start/")]
        public IActionResult Head(Guid actionPlanId, Guid interactionId, Guid objId, int objectUpdated, int propertyUpdated)
        {
            return View();
        }
        [Route("/bodytop/start/")]
        public IActionResult BodyTop()
        {
            return View();
        }
        [Route("/breadcrumb/start/")]
        public IActionResult Breadcrumb(Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            return View();
        }
      

        [Route("/bodyfooter/start/")]
        public  IActionResult BodyFooter()
        {
            return View();
        }
    }

}
