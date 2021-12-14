using System;
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

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class ChangeGoalStatusController : CompositeSessionController<ChangeGoalCompositeViewModel>
    {
        private readonly IDssWriter _dssWriter;
        private readonly IDssReader _dssReader;

        public ChangeGoalStatusController(ILogger<ChangeGoalStatusController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, IDssWriter dssWriter, ICosmosService cosmosServiceService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssWriter = dssWriter;
            _dssReader = dssReader;
            ViewModel.GeneratePageTitle("Change goal status");
        }

        [Route("/body/change-goal-status")]
        [HttpGet]
        public async Task<IActionResult> Body( Guid goalId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            if (customer == null || session == null)
            {
                return BadRequest("unable to get customer details");
            }
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);

            ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), goalId.ToString());

            return await base.Body();
        }


        [Route("/body/change-goal-status")]
        [HttpPost]
        public async Task<IActionResult> Body(ChangeGoalCompositeViewModel model, IFormCollection formCollection)
        {
            InitVM(model);

            GoalStatus newGoalStatus;

            if (Enum.TryParse(formCollection["GoalStatus"], true, out newGoalStatus))
            {
                ViewModel.Goal = new Goal()
                {
                    GoalId = model.Goal.GoalId,
                    GoalStatus = newGoalStatus,
                    DateGoalShouldBeCompletedBy = model.Goal.DateGoalShouldBeCompletedBy
                };

                await UpdateGoal();
                return RedirectTo(Urls.GetUpdateConfirmationUrl(new Guid(ViewModel.Goal.GoalId), Constants.Constants.Goal, Constants.Constants.Status));
            }
            else
            {
                model.ErrorMessage = "Choose a status for this goal or select ‘Cancel’ to view it.";
            }

            ModelState.Clear();
            ModelState.AddModelError(Constants.Constants.GoalStatus, model.ErrorMessage);

            var customer = await GetCustomerDetails();
            if (customer == null)
            {
                return BadRequest("unable to get customer details");
            }
            await ManageSession(customer.CustomerId, model.ActionPlanId, model.InteractionId);
            return await base.Body();
        }

        public override IActionResult Breadcrumb(Guid goalId)
        {
            return base.Breadcrumb(goalId);
        }

        private void InitVM(ChangeGoalCompositeViewModel model)
        {
            ViewModel.ActionPlanId = model.ActionPlanId;
            ViewModel.InteractionId = model.InteractionId;
            ViewModel.CustomerId = model.CustomerId;
            ViewModel.DateGoalShouldBeCompletedBy = model.DateGoalShouldBeCompletedBy;
            ViewModel.Goal = new Goal()
            {
                GoalId = model.Goal.GoalId,
                DateGoalShouldBeCompletedBy = model.Goal.DateGoalShouldBeCompletedBy
            };
        }

        private async Task UpdateGoal()
        {
            var updateGoal = new UpdateGoal()
            {
                CustomerId = ViewModel.CustomerId,
                InteractionId = ViewModel.InteractionId,
                ActionPlanId = ViewModel.ActionPlanId,
                GoalId = new Guid(ViewModel.Goal.GoalId),
                DateGoalShouldBeCompletedBy = ViewModel.Goal.DateGoalShouldBeCompletedBy,
                GoalStatus = ViewModel.Goal.GoalStatus
            };
            await _dssWriter.UpdateGoal(updateGoal);
        }
    }
}