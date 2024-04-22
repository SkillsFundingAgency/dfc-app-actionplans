﻿using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.App.ActionPlans.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class ChangeActionStatusController : CompositeSessionController<ChangeActionCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;

        public ChangeActionStatusController(ILogger<ChangeActionStatusController> logger,
            IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService,
           ISharedContentRedisInterface sharedContentRedis, IConfiguration config)
            : base(compositeSettings, dssReader, cosmosServiceService, sharedContentRedis, config)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            ViewModel.GeneratePageTitle("Change action status");
        }

        [Route("/body/change-action-status")]
        [HttpGet]
        public async Task<IActionResult> Body( Guid actionId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            if (customer == null || session == null)
            {
                return BadRequest("unable to get customer details");
            }
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);

            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());

            return await base.Body();
        }

        [Route("/body/change-action-status")]
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
                return RedirectTo(Urls.GetUpdateConfirmationUrl( new Guid(ViewModel.Action.ActionId), Constants.Constants.Action,
                    Constants.Constants.Status));
            }
            else
            {
                model.ErrorMessage = "Choose a status for this action or select ‘Cancel’ to view it.";
            }

            ModelState.Clear();
            ModelState.AddModelError(Constants.Constants.ActionStatus, model.ErrorMessage);

            var customer = await GetCustomerDetails();
            if (customer == null)
            {
                return BadRequest("unable to get customer details");
            }
            await ManageSession(customer.CustomerId, model.ActionPlanId, model.InteractionId);
            return await base.Body();
        }

        public override IActionResult Breadcrumb(Guid actionId)
        {
            return base.Breadcrumb(actionId);
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