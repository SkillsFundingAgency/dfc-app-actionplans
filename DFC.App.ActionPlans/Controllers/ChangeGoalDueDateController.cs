using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class ChangeGoalDueDateController: CompositeSessionController<ChangeGoalDueDateCompositeViewModel>
    {
        
            private readonly IDssWriter _dssWriter;

            public ChangeGoalDueDateController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
                :base(compositeSettings, dssReader)
            {
                _dssWriter = dssWriter;
            }
        
            //  [Authorize]
            [Route("/body/change-goal-due-date/{actionPlanId}/{interactionId}/{goalId}")]
            [HttpGet]
            public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
            {
                ViewModel.Goal = new Goal(){GoalId  = goalId.ToString()};

                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, actionPlanId, interactionId);
                
                return await base.Body();
            }

            [Route("/body/change-goal-due-date")]
            [HttpPost]
            public async  Task<IActionResult> Body(ChangeGoalDueDateCompositeViewModel model, IFormCollection formCollection)
            {

                #region Setup ViewModel

                ViewModel.ActionPlanId = model.ActionPlanId;
                ViewModel.InteractionId = model.InteractionId;
                ViewModel.Goal = new Goal(){GoalId = model.Goal.GoalId};
                ViewModel.DateGoalShouldBeCompletedBy = new SplitDate()
                {
                    Day = formCollection["Day"],
                    Month = formCollection["Month"],
                    Year = formCollection["Year"]
                };

                #endregion

                if (!ViewModel.DateGoalShouldBeCompletedBy.isEmpty())
                {

                    DateTime dateValue;
                    if (Validate.CheckValidSplitDate(ViewModel.DateGoalShouldBeCompletedBy, out dateValue))
                    {

                        if (Validate.CheckValidDueDate(ViewModel.DateGoalShouldBeCompletedBy, out dateValue))
                        {
                            await UpdateGoal(model, dateValue);
                            return RedirectTo($"{CompositeViewModel.PageId.UpdateGoalConfirmation}/{ViewModel.ActionPlanId}/{ViewModel.InteractionId}/{ViewModel.Goal.GoalId}");
                        }

                        model.ErrorMessage = "The goal due date must be today or in the future";
                    }
                    else
                    {
                        model.ErrorMessage = "The goal due date must be a real date";
                    }
                }
                else
                {
                    model.ErrorMessage = "Enter the date that you would like to achieve this goal";
                }

                ModelState.Clear(); //Remove model binding errors as we will check if the date is valid  or not.
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
                   
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
                return await base.Body();
            }

            private async Task UpdateGoal(ChangeGoalDueDateCompositeViewModel model, DateTime dateValue)
            {
                var updateGoal = new UpdateGoal()
                {
                    CustomerId = model.CustomerId,
                    InteractionId = model.InteractionId,
                    ActionPlanId = model.ActionPlanId,
                    GoalId = new Guid(model.Goal.GoalId),
                    DateGoalShouldBeCompletedBy = dateValue
                };
                await _dssWriter.UpdateGoal(updateGoal);
            }
    }
}
