using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Helpers;
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
        public UpdateConfirmationController(ILogger<UpdateConfirmationController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService)
            : base(compositeSettings, dssReader, cosmosServiceService)
        {
            _dssReader = dssReader;
        }

        
        [Route("/body/update-confirmation")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid objectId, int objectUpdated, int propertyUpdated)
        {
            var customer = await GetCustomerDetails();
            var session = await base.GetUserSession();
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            await SetUpdateMessages(objectId, objectUpdated, propertyUpdated);
            return await base.Body();
        }
        
        [Route("/head/update-confirmation")]
        [HttpGet]
        public override IActionResult Head()
        {
            var queryobjectUpdated = string.IsNullOrEmpty(Request.Query["objectUpdated"]) ? "0"
                : Request.Query["objectUpdated"].ToString();
            int.TryParse(queryobjectUpdated, out var objectUpdated);

           var title = objectUpdated switch
            {
                Constants.Constants.Goal => "Goal Updated",
                Constants.Constants.Action => "Action Updated",
                _ => throw new ObjectUpdatedNotSetException($"Object updated has not been provided or is incorrect.")
            };
           ViewModel.GeneratePageTitle(title);
            return  base.Head();
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
            ViewModel.ObjLink =  Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, goalId);
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
            
            var action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());
            ViewModel.Name = $"{action.ActionSummary} - {action.ActionType.GetDisplayName()}";
            ViewModel.ObjLink = Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path, actionId);
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
                        $"You have changed the status of this action to <strong>{action.ActionStatus.GetDisplayName()}</strong>.";
                    break;
                default:
                    throw new PropertyUpdatedNotSetException(
                        $"Property updated has not been provided or is incorrect for Action. {ViewModel.Name}");
            }
        }
    }
}
