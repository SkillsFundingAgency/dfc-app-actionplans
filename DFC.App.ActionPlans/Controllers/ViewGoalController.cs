using System;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    [Authorize]
    public class ViewGoalController : CompositeSessionController<ViewGoalCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        
        public ViewGoalController(ILogger<ViewGoalController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, ICosmosService cosmosServiceService,
            IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssReader = dssReader;
        }
        
        [Route("/body/view-goal")]
        [HttpGet]
        public async  Task<IActionResult> Body(Guid goalId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            ViewModel.Goal = await _dssReader.GetGoalDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(),goalId.ToString());
            SetBackLink();
            return await base.Body();
        }

        private void  SetBackLink()
        {
            ViewModel.BackLink = Urls.GetViewActionPlanUrl(ViewModel.CompositeSettings.Path);
        }
        
    }
}
