using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ViewActionController : CompositeSessionController<ViewActionCompositeViewModel>
    {
        private readonly IDssReader _dssReader;

        public ViewActionController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader)
            : base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }

        //  [Authorize]
        [Route("/body/view-action/{actionPlanId}/{interactionId}/{actionId}")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid actionId)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());
            return await base.Body();
        }
    }
}
