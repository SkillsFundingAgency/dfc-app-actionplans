using System;
using System.Threading.Tasks;
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

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class ChangeGoalDueDateController : CompositeSessionController<ChangeGoalCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;

        public ChangeGoalDueDateController(ILogger<ChangeGoalDueDateController> logger,
            IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService,
            IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            ViewModel.GeneratePageTitle("Change goal due date");
        }

        [Route("/body/change-goal-due-date")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid goalId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(), session.InteractionId.ToString(),
                session.ActionPlanId.ToString(), goalId.ToString());
            return await base.Body();
        }

    
        [Route("/body/change-goal-due-date")]
        [HttpPost]
        public async Task<IActionResult> Body(ChangeGoalCompositeViewModel model, IFormCollection formCollection)
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
                        return RedirectTo(Urls.GetUpdateConfirmationUrl(new Guid(ViewModel.Goal.GoalId), Constants.Constants.Goal,
                            Constants.Constants.Date));
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
            await ManageSession(customer.CustomerId, model.ActionPlanId, model.InteractionId);
            return await base.Body();
        }

        public override IActionResult Breadcrumb(Guid goalId)
        {
            return base.Breadcrumb(goalId);
        }

        private void InitVM(ChangeGoalCompositeViewModel model)
        {
            ViewModel.CustomerId = model.CustomerId;
            ViewModel.ActionPlanId = model.ActionPlanId;
            ViewModel.InteractionId = model.InteractionId;
            ViewModel.Goal = new Goal()
            {
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
    }
}