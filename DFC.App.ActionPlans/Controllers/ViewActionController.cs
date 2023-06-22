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
    public class ViewActionController : CompositeSessionController<ViewActionCompositeViewModel>
    {
        private readonly IDssReader _dssReader;
        private readonly ILogger _dsslogger;

        public ViewActionController(ILogger<ViewActionController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader dssReader, ICosmosService cosmosServiceService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(logger,compositeSettings, dssReader, cosmosServiceService, documentService, config)
        {
            _dssReader = dssReader;
            _dsslogger = logger;
        }

        [Route("/body/view-action")]
        [HttpGet]
        public async Task<IActionResult> Body(Guid actionId)
        {
            var session = await GetUserSession();
            var customer = await GetCustomerDetails();
            if (customer == null || session == null)
            {
                _dsslogger.LogError($"ViewActionController Body Customer or session is null actionId {actionId}");

                return BadRequest("unable to get customer details");
            }
            await ManageSession(customer.CustomerId, session.ActionPlanId, session.InteractionId);
            ViewModel.Action = await _dssReader.GetActionDetails(ViewModel.CustomerId.ToString(),
                ViewModel.InteractionId.ToString(), ViewModel.ActionPlanId.ToString(), actionId.ToString());
            SetBackLink();
            return await base.Body();
        }

        private void  SetBackLink()
        {
            _dsslogger.LogError($"ViewActionController SetBackLink {ViewModel.CompositeSettings.Path}");
            ViewModel.BackLink = Urls.GetViewActionPlanUrl(ViewModel.CompositeSettings.Path);
        }
    }
}
