using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Threading.Tasks;
using DFC.APP.ActionPlans.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.Extensions.Configuration;

namespace DFC.App.ActionPlans.Controllers
{
    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        private readonly CompositeSettings _configuration;
        private readonly ILogger<ErrorController> _dsslogger;
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader _dssReader, ICosmosService cosmosServiceService, IOptions<CompositeSettings> configuration,
            IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(logger,compositeSettings, _dssReader, cosmosServiceService, documentService, config)
        {
            _configuration = configuration.Value;
            _dsslogger = logger;
        }

        [Route("/body/Error")]
        [HttpGet]
        public async Task<IActionResult> Body()
        {
            ViewModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewModel.ShowEnhancedLog = _configuration.EnhancedError;
            _dsslogger.LogError($"ActionPlans /body/Error ViewModel.RequestId {ViewModel.RequestId}");
            return await base.Body();
        }
    }
}