using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Personalisation.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class UpdateConfirmationController : CompositeSessionController<UpdateGoalConfirmationCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        public UpdateConfirmationController(ILogger<UpdateConfirmationController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssReader = dssReader;
        }


        [Route("/body/update-confirmation")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid objectId, int objectUpdated, int propertyUpdated)
        {
            var customer = await GetCustomerDetails();
            var session = await base.GetUserSession();
            if (customer == null || session == null)
            {
                return BadRequest("unable to get customer details");
            }

            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            if (objectUpdated != Constants.Constants.Goal && objectUpdated != Constants.Constants.Action ||
                propertyUpdated != Constants.Constants.Date && propertyUpdated != Constants.Constants.Status)
            {
                return BadRequest("Object updated has not been provided or is incorrect.");
            }
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

            var title = string.Empty;

            switch (objectUpdated)
            {
                case Constants.Constants.Goal:
                    title = "Goal updated";
                    break;
                case Constants.Constants.Action:
                        title = "Action updated";
                    break;
                default:
                    return BadRequest("Object updated has not been provided or is incorrect.");

            }

            ViewModel.GeneratePageTitle(title);
            return base.Head();
        }

        private async Task SetUpdateMessages(Guid objId, int objectUpdated, int propertyUpdated)
        {

            switch (objectUpdated)
            {
                case Constants.Constants.Goal:
                    await SetGoalUpdateMessages(objId, propertyUpdated);
                    break;

                default:
                    await SetActionUpdateMessages(objId, propertyUpdated);
                    break;
            }
        }

        private async Task SetGoalUpdateMessages(Guid goalId, int propertyUpdated)
        {
            ViewModel.PageTitle = "Goal Updated";
            var goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());

            ViewModel.Name = $"{goal.GoalType.GetDisplayName()} - ";
            ViewModel.Summary = $"{goal.GoalSummary}";
            ViewModel.ObjLink = Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, goalId);
            ViewModel.ObjLinkText = "view or update this goal";
            SetGoalMessagesForProperty(propertyUpdated, goal);
        }

        private void SetGoalMessagesForProperty(int propertyUpdated, Goal goal)
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
                default:
                    {
                        ViewModel.WhatChanged = "Goal status updated";
                        ViewModel.UpdateMessage =
                            $"You have changed the status of this goal to <strong>{goal.GoalStatus.GetDisplayName()}</strong>.";
                        break;
                    }
            }
        }

        private async Task SetActionUpdateMessages(Guid actionId, int propertyUpdated)
        {

            var action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());
            ViewModel.Name = $"{action.ActionType.GetDisplayName()} - ";
            ViewModel.Summary = $"{action.ActionSummary}";
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
                default:
                    ViewModel.WhatChanged = "Action status updated";
                    ViewModel.UpdateMessage =
                        $"You have changed the status of this action to <strong>{action.ActionStatus.GetDisplayName()}</strong>.";
                    break;
            }
        }
    }
}
