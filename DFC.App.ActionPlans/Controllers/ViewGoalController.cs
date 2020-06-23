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
    public class ViewGoalController : CompositeSessionController<ViewGoalCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        
        public ViewGoalController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader)
            :base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }
        
        //  [Authorize]
        [Route("/body/view-goal/{actionPlanId}/{interactionId}/{goalId}")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
            ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(),goalId.ToString());
            return await base.Body();
        }


        
    }
}
