using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Constants;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class ChangeGoalStatusController : CompositeSessionController<ChangeGoalCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;

        public ChangeGoalStatusController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
            :base(compositeSettings, dssReader)
        {
            _dssWriter = dssWriter;
            ViewModel.PageTitle = "Change Goal Status";
        }

          //  [Authorize]
            [Route("/body/change-goal-status/{actionPlanId}/{interactionId}/{goalId}")]
            [HttpGet]
            public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
            {
                ViewModel.Goal = new Goal(){GoalId  = goalId.ToString()};

                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, actionPlanId, interactionId);
                
                return await base.Body();
            }

            [Route("/body/change-goal-status")]
            [HttpPost]
            public async  Task<IActionResult> Body(ChangeGoalCompositeViewModel model, IFormCollection formCollection)
            {
                ViewModel.ActionPlanId = model.ActionPlanId;
                ViewModel.InteractionId = model.InteractionId;
                GoalStatus newGoalStatus;
                if (Enum.TryParse(formCollection["GoalStatus"], true, out newGoalStatus))
                {

                    ViewModel.Goal = new Goal()
                    {
                        GoalId = model.Goal.GoalId,
                        GoalStatus = newGoalStatus
                    };
                    await UpdateGoal(model);
                    return RedirectTo($"{CompositeViewModel.PageId.UpdateGoalConfirmation}/{ViewModel.ActionPlanId}/{ViewModel.InteractionId}/{ViewModel.Goal.GoalId}");
                }
                else
                {
                    model.ErrorMessage = "Choose a status for this goal or select ‘Cancel’ to view it.";
                }

                ModelState.Clear(); //Remove model binding errors as we will check if the date is valid  or not.
                ModelState.AddModelError(Constants.Constants.GoalStatus, model.ErrorMessage);
                   
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
                return await base.Body();
            }

            private async Task UpdateGoal(ChangeGoalCompositeViewModel model)
            {
                var updateGoal = new UpdateGoal()
                {
                    CustomerId = model.CustomerId,
                    InteractionId = model.InteractionId,
                    ActionPlanId = model.ActionPlanId,
                    GoalId = new Guid(model.Goal.GoalId),
                    DateGoalShouldBeCompletedBy = model.Goal.DateGoalShouldBeCompletedBy,
                    GoalStatus = model.Goal.GoalStatus
                };
                await _dssWriter.UpdateGoal(updateGoal);
            }
    }
}
