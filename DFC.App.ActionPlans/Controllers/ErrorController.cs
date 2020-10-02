using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class ErrorController : CompositeSessionController<ErrorCompositeViewModel>
    {
        public ErrorController(ILogger<ErrorController> logger, IOptions<CompositeSettings> compositeSettings,
            IDssReader _dssReader, ICosmosService cosmosServiceService)
            : base(compositeSettings, _dssReader, cosmosServiceService)
        {
            
        }
    }
}