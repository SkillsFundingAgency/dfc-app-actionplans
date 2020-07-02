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
        private readonly IDssReader _dssReader;
        public UpdateGoalConfirmationController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader)
            : base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }

        
        [Route("/body/update-goal-confirmation/{actionPlanId}/{interactionId}/{goalId}/{objectupdated}/{propertyupdated}")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId, int objectUpdated, int propertyUpdated)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
            if (objectUpdated == Constants.Constants.Goal)
            {
                ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(),
                    ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());
                ViewModel.PropertyUpdated = propertyUpdated;
            }
            
            return await base.Body();
        }
    }
}
