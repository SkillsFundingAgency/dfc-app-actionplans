﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
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
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.Controllers
{
    public class ChangeActionStatusController : CompositeSessionController<ChangeActionCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;

        public ChangeActionStatusController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
            :base(compositeSettings, dssReader)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            ViewModel.PageTitle = "Change Action Status";
        }

        //  [Authorize]
        [Route("/body/change-action-status/{actionPlanId}/{interactionId}/{goalId}")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
        {
            await LoadViewData(actionPlanId, interactionId);

            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());

            return await base.Body();
        }

          [Route("/body/change-goal-status/{actionPlanId}/{interactionId}/{goalId}")]
        [HttpPost]
        public async Task<IActionResult> Body(ChangeActionCompositeViewModel model, IFormCollection formCollection)
        {
            InitVM(model);

            ActionStatus newActionStatus;

            if (Enum.TryParse(formCollection["ActionStatus"], true, out newActionStatus))
            {
                ViewModel.Action = new Action()
                {
                    ActionId = model.Action.ActionId,
                    ActionStatus = newActionStatus,
                    DateActionAimsToBeCompletedBy = model.Action.DateActionAimsToBeCompletedBy
                };

                await UpdateAction();
                return RedirectTo(Links.GetUpdateConfirmationLink(ViewModel.ActionPlanId,
                    ViewModel.InteractionId, new Guid(ViewModel.Action.ActionId), Constants.Constants.Action,
                    Constants.Constants.Status));
            }
            else
            {
                model.ErrorMessage = "Choose a status for this action or select ‘Cancel’ to view it.";
            }

            ModelState.Clear(); 
            ModelState.AddModelError(Constants.Constants.GoalStatus, model.ErrorMessage);

            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
            return await base.Body();
        }

        private async Task LoadViewData(Guid actionPlanId, Guid interactionId)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
        }

        private void InitVM(ChangeActionCompositeViewModel model)
        {
            ViewModel.CustomerId = model.CustomerId;
            ViewModel.ActionPlanId = model.ActionPlanId;
            ViewModel.InteractionId = model.InteractionId;
            ViewModel.Action = new Action()
            {
                ActionId = model.Action.ActionId,
                ActionStatus = model.Action.ActionStatus
            };
        }

        private async Task UpdateAction()
        {
            var updateAction = new UpdateAction()
            {
                CustomerId = ViewModel.CustomerId,
                InteractionId = ViewModel.InteractionId,
                ActionPlanId = ViewModel.ActionPlanId,
                ActionId = new Guid(ViewModel.Action.ActionId),
                DateActionAimsToBeCompletedBy = ViewModel.Action.DateActionAimsToBeCompletedBy,
                ActionStatus = ViewModel.Action.ActionStatus
            };
            await _dssWriter.UpdateAction(updateAction);
        }
    }
}
