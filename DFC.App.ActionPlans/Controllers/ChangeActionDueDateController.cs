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
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.Controllers
{
    public class ChangeActionDueDateController : CompositeSessionController<ChangeActionCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;

        public ChangeActionDueDateController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter)
            :base(compositeSettings, dssReader)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            ViewModel.PageTitle = "Change Action due date";
        }

        //  [Authorize]
        [Route("/body/change-action-due-date/{actionPlanId}/{interactionId}/{Id}")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid id)
        {
            var customer = await GetCustomerDetails();
            await LoadData(customer.CustomerId, actionPlanId, interactionId);
            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(), interactionId.ToString(), actionPlanId.ToString(), id.ToString());
            BackLink();
            return await base.Body();
        }

           [Route("/body/change-action-due-date")]
            [HttpPost]
            public async  Task<IActionResult> Body(ChangeActionCompositeViewModel model, IFormCollection formCollection)
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
                            return RedirectTo( Links.GetUpdateConfirmationLink(ViewModel.ActionPlanId, ViewModel.InteractionId, new Guid( ViewModel.Action.ActionId), Constants.Constants.Action, Constants.Constants.Date));
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

                ModelState.Clear(); //Remove model binding errors as we will check if the date is valid  or not.
                ModelState.AddModelError(Constants.Constants.DateGoalShouldBeCompletedBy, model.ErrorMessage);
                   
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
                BackLink();
                return await base.Body();
            }


            private void InitVM(ChangeActionCompositeViewModel model)
            {
                ViewModel.CustomerId = model.CustomerId;
                ViewModel.ActionPlanId = model.ActionPlanId;
                ViewModel.InteractionId = model.InteractionId;
                ViewModel.Action = new Action(){
                     ActionId= model.Action.ActionId,
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

            private void  BackLink()
            {
                ViewModel.BackLink = @Links.GetViewActionLink(ViewModel.CompositeSettings.Path, ViewModel.ActionPlanId, ViewModel.InteractionId, new Guid(ViewModel.Action.ActionId));
            }
    }
}
