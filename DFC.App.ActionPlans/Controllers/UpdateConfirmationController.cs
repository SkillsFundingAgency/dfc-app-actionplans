using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using DFC.Personalisation.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class UpdateConfirmationController : CompositeSessionController<UpdateGoalConfirmationCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        public UpdateConfirmationController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader)
            : base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }

        
        [Route("/body/update-goal-confirmation/{actionPlanId}/{interactionId}/{objId}/{objectupdated}/{propertyupdated}")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid objId, int objectUpdated, int propertyUpdated)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
            await SetUpdateMessages(objId,objectUpdated, propertyUpdated);
            return await base.Body();
        }

        private async Task SetUpdateMessages(Guid objId, int objectUpdated, int propertyUpdated)
        {

            switch (objectUpdated)
            {
                case Constants.Constants.Goal:
                    await SetGoalUpdateMessages(objId, propertyUpdated);
                    break;

                case Constants.Constants.Action:
                    await SetActionUpdateMessages(objId, propertyUpdated);
                    break;
                
                default:
                    throw new ObjectUpdatedNotSetException(
                        $"Object updated has not been provided or is incorrect.");
            }
        }

        private async Task SetGoalUpdateMessages(Guid goalId, int propertyUpdated)
        {
            var Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());
            
            ViewModel.Name = $"{Goal.GoalSummary} - {Goal.GoalType.GetDisplayName()}";
            ViewModel.ObjLink = $"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.ViewGoal}/{ViewModel.ActionPlanId}/{ViewModel.InteractionId}/{goalId}";
            ViewModel.ObjLinkText = "view or update this goal";
            
            switch (propertyUpdated)
            {
                case Constants.Constants.Date:
                {
                    ViewModel.WhatChanged = "Due date changed";
                    ViewModel.UpdateMessage = $"You have changed the due date of this goal to <strong>{Goal.DateGoalShouldBeCompletedBy.DateOnly()}</strong>.";
                    break;
                }
                case Constants.Constants.Status:
                {
                    ViewModel.WhatChanged = "Goal status updated";
                    ViewModel.UpdateMessage = $"You have changed the status of this goal. <strong>{Goal.GoalStatus.GetDisplayName()}</strong>.";
                    break;
                }
                default:
                {
                    throw new PropertyUpdatedNotSetException($"Property updated has not been provided or is incorrect for Goal. {ViewModel.Name}");
                }
            }
        }

        private async Task SetActionUpdateMessages(Guid goalId, int propertyUpdated)
        {
            var Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());
            ViewModel.Name = Action.ActionSummary;

            switch (propertyUpdated)
            {
                case Constants.Constants.Date:
                    ViewModel.WhatChanged = "Due date changed";
                    ViewModel.UpdateMessage =
                        $"You have changed the due date of this action to {Action.DateActionAimsToBeCompletedBy.DateOnly()}.";
                    break;
                case Constants.Constants.Status:
                    ViewModel.WhatChanged = "Action status updated";
                    ViewModel.UpdateMessage =
                        $"You have changed the status of this action. {Action.ActionStatus.GetDisplayName()}.";
                    break;
                default:
                    throw new PropertyUpdatedNotSetException(
                        $"Property updated has not been provided or is incorrect for Action. {ViewModel.Name}");
            }
        }
    }
}
