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
    public class ChangeGoalDueDateController: CompositeSessionController<ChangeGoalCompositeViewModel>
    {
        
            private readonly IDssWriter _dssWriter;
            private readonly IDssReader _dssReader;

            public ChangeGoalDueDateController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
                :base(compositeSettings, dssReader)
            {
                _dssWriter = dssWriter;
                _dssReader = dssReader;
                ViewModel.PageTitle = "Change Goal due date";
            }
        
            //  [Authorize]
            [Route("/body/change-goal-due-date/{actionPlanId}/{interactionId}/{goalId}")]
            [HttpGet]
            public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
            {
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, actionPlanId, interactionId);
                ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(), interactionId.ToString(), actionPlanId.ToString(), goalId.ToString());
                SetBackLink();
                return await base.Body();
            }

            [Route("/body/change-goal-due-date")]
            [HttpPost]
            public async  Task<IActionResult> Body(ChangeGoalCompositeViewModel model, IFormCollection formCollection)
            {


                InitVM(model);

                ViewModel.DateGoalShouldBeCompletedBy = new SplitDate()
                {
                    Day = formCollection["Day"],
                    Month = formCollection["Month"],
                    Year = formCollection["Year"]
                };

                if (!ViewModel.DateGoalShouldBeCompletedBy.isEmpty())
                {
                    DateTime dateValue;
                    if (Validate.CheckValidSplitDate(ViewModel.DateGoalShouldBeCompletedBy, out dateValue))
                    {

                        if (Validate.CheckValidDueDate(ViewModel.DateGoalShouldBeCompletedBy, out dateValue))
                        {
                            await UpdateGoal(dateValue);
                            return RedirectTo(Links.GetUpdateConfirmationLink(ViewModel.ActionPlanId, ViewModel.InteractionId, new Guid( ViewModel.Goal.GoalId), Constants.Constants.Goal, Constants.Constants.Date));
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
                ModelState.AddModelError(Constants.Constants.DateGoalShouldBeCompletedBy, model.ErrorMessage);
                   
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
                return await base.Body();
            }

            private void InitVM(ChangeGoalCompositeViewModel model)
            {
                ViewModel.CustomerId = model.CustomerId;
                ViewModel.ActionPlanId = model.ActionPlanId;
                ViewModel.InteractionId = model.InteractionId;
                ViewModel.Goal = new Goal(){
                    GoalId = model.Goal.GoalId,
                    GoalStatus = model.Goal.GoalStatus
                };
            }

            private async Task UpdateGoal(DateTime dateValue)
            {
                var updateGoal = new UpdateGoal()
                {
                    CustomerId = ViewModel.CustomerId,
                    InteractionId = ViewModel.InteractionId,
                    ActionPlanId = ViewModel.ActionPlanId,
                    GoalId = new Guid(ViewModel.Goal.GoalId),
                    DateGoalShouldBeCompletedBy = dateValue,
                    GoalStatus = ViewModel.Goal.GoalStatus
                };
                await _dssWriter.UpdateGoal(updateGoal);
            }

            private void  SetBackLink()
            {
                ViewModel.BackLink = @Links.GetViewGoalLink(ViewModel.CompositeSettings.Path, ViewModel.ActionPlanId, ViewModel.InteractionId, new Guid(ViewModel.Goal.GoalId));
            }
    }
}
