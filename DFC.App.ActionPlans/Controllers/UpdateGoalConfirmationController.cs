using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class UpdateGoalConfirmationController : CompositeSessionController<UpdateGoalConfirmationCompositeViewModel>
    {
        public UpdateGoalConfirmationController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader)
            : base(compositeSettings, dssReader)
        {
        }

        [Route("/body/")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId)
        {
            return await base.Body();
        }
    }
}
