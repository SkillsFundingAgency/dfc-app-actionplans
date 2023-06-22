using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Extensions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
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

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class ChangeActionDueDateController : CompositeSessionController<ChangeActionCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;
        private readonly ILogger _dsslogger;
        public ChangeActionDueDateController(ILogger logger,
            IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService,
            IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(logger,compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            _dsslogger = logger;
            ViewModel.GeneratePageTitle("Change action due date");
        }

        [Route("/body/change-action-due-date")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            if (customer == null || session == null)
            {
                _dsslogger.LogError($"ChangeActionDueDateController Body Customer or session is null actionId {actionId}");
                return BadRequest("unable to get customer details");
            }

            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                session.InteractionId.ToString(), session.ActionPlanId.ToString(), actionId.ToString());

            _dsslogger.LogInformation($"ChangeActionDueDateController Body actionId {actionId} CustomerId {customer.CustomerId} ");

            return await base.Body();
        }

        [Route("/body/change-action-due-date")]
        [HttpPost]
        public async Task<IActionResult> Body(ChangeActionCompositeViewModel model, IFormCollection formCollection)
        {
            InitVM(model);

            ViewModel.DateActionShouldBeCompletedBy = new SplitDate()
            {
                Day = formCollection["Day"],
                Month = formCollection["Month"],
                Year = formCollection["Year"]
            };

            if (!ViewModel.DateActionShouldBeCompletedBy.isEmpty())
            {
                DateTime dateValue;
                if (Validate.CheckValidSplitDate(ViewModel.DateActionShouldBeCompletedBy, out dateValue))
                {
                    if (Validate.CheckValidDueDate(ViewModel.DateActionShouldBeCompletedBy, out dateValue))
                    {
                        await UpdateAction(dateValue);
                        return RedirectTo(Urls.GetUpdateConfirmationUrl(new Guid(ViewModel.Action.ActionId), Constants.Constants.Action,
                            Constants.Constants.Date));
                    }

                    model.ErrorMessage = "The action due date must be today or in the future";
                }
                else
                {
                    model.ErrorMessage = "The action due date must be a real date";
                }
            }
            else
            {
                model.ErrorMessage = "Enter the date that you would like to complete this action by";
            }

            ModelState.Clear();
            ModelState.AddModelError(Constants.Constants.DateActionShouldBeCompletedBy, model.ErrorMessage);

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

        private async Task UpdateAction(DateTime dateValue)
        {
            var updateAction = new UpdateAction()
            {
                CustomerId = ViewModel.CustomerId,
                InteractionId = ViewModel.InteractionId,
                ActionPlanId = ViewModel.ActionPlanId,
                ActionId = new Guid(ViewModel.Action.ActionId),
                DateActionAimsToBeCompletedBy = dateValue,
                ActionStatus = ViewModel.Action.ActionStatus
            };
            await _dssWriter.UpdateAction(updateAction);
        }
    }
}