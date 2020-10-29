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
using Microsoft.Extensions.Configuration;

namespace DFC.App.ActionPlans.Controllers
{
    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        private readonly CompositeSettings _configuration;
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader _dssReader, ICosmosService cosmosServiceService, IOptions<CompositeSettings> configuration)
            : base(compositeSettings, _dssReader, cosmosServiceService)
        {
            _configuration = configuration.Value;
        }

        [Route("/body/Error")]
        [HttpGet]
        public async Task<IActionResult> Body()
        {
            ViewModel.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewModel.ShowEnhancedLog = _configuration.EnhancedError;
            return await base.Body();
        }
    }
}