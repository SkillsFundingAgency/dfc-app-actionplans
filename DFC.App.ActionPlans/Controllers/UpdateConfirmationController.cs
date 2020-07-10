using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using DFC.Personalisation.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class UpdateConfirmationController : CompositeSessionController<UpdateGoalConfirmationCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        public UpdateConfirmationController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService)
            : base(compositeSettings, dssReader, cosmosServiceService)
        {
            _dssReader = dssReader;
        }

        
        [Route("/body/update-confirmation/{actionPlanId}/{interactionId}/{objId}/{objectupdated}/{propertyupdated}")]
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
            ViewModel.PageTitle = "Goal Updated";
            var goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());
            
            ViewModel.Name = $"{goal.GoalSummary} - {goal.GoalType.GetDisplayName()}";
            ViewModel.ObjLink = $"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.ViewGoal}/{ViewModel.ActionPlanId}/{ViewModel.InteractionId}/{goalId}";
            ViewModel.ObjLinkText = "view or update this goal";
            SetGoalMessagesForProperty(propertyUpdated, goal);
        }

        private void SetGoalMessagesForProperty(int propertyUpdated,Goal goal)
        {
            switch (propertyUpdated)
            {
                case Constants.Constants.Date:
                {
                    ViewModel.WhatChanged = "Due date changed";
                    ViewModel.UpdateMessage =
                        $"You have changed the due date of this goal to <strong>{goal.DateGoalShouldBeCompletedBy.DateOnly()}</strong>.";
                    break;
                }
                case Constants.Constants.Status:
                {
                    ViewModel.WhatChanged = "Goal status updated";
                    ViewModel.UpdateMessage =
                        $"You have changed the status of this goal. <strong>{goal.GoalStatus.GetDisplayName()}</strong>.";
                    break;
                }
                default:
                {
                    throw new PropertyUpdatedNotSetException(
                        $"Property updated has not been provided or is incorrect for Goal. {ViewModel.Name}");
                }
            }
        }

        private async Task SetActionUpdateMessages(Guid actionId, int propertyUpdated)
        {
            ViewModel.PageTitle = "Action Updated";
            var action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());
            ViewModel.Name = action.ActionSummary;
            ViewModel.ObjLink = $"{ViewModel.CompositeSettings.Path}/{CompositeViewModel.PageId.ViewAction}/{ViewModel.ActionPlanId}/{ViewModel.InteractionId}/{actionId}";
            ViewModel.ObjLinkText = "view or update this action";
            SetActionStatusMessages(propertyUpdated, action);

        }

        private void SetActionStatusMessages(int propertyUpdated, Services.DSS.Models.Action action)
        {
            switch (propertyUpdated)
            {
                case Constants.Constants.Date:
                    ViewModel.WhatChanged = "Due date changed";
                    ViewModel.UpdateMessage =
                        $"You have changed the due date of this action to <strong>{action.DateActionAimsToBeCompletedBy.DateOnly()}</strong>.";
                    break;
                case Constants.Constants.Status:
                    ViewModel.WhatChanged = "Action status updated";
                    ViewModel.UpdateMessage =
                        $"You have changed the status of this action. <strong>{action.ActionStatus.GetDisplayName()}</strong>.";
                    break;
                default:
                    throw new PropertyUpdatedNotSetException(
                        $"Property updated has not been provided or is incorrect for Action. {ViewModel.Name}");
            }
        }
    }
}
