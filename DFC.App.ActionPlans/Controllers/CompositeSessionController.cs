using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using Microsoft.Extensions.Configuration;
using Constants = DFC.App.ActionPlans.Constants.Constants;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;

namespace Dfc.App.ActionPlans.Controllers
{
  
    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
    /// 
    [ExcludeFromCodeCoverage]
    public abstract class CompositeSessionController<TViewModel>:SessionController where TViewModel : CompositeViewModel, new()
    {
        private readonly IDssReader _dssReader;
        protected TViewModel ViewModel { get; }
        //private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public const string SharedContectStaxId = "c0117ac7-115a-4bc1-9350-3fb4b00c7857";
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, ICosmosService cosmosServiceService,  ISharedContentRedisInterface sharedContentRedis, IConfiguration config)
            : base(cosmosServiceService)        
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };  
            _dssReader = dssReader;
            //_documentService = documentService;
            _sharedContent = config.GetValue<Guid>(DFC.APP.ActionPlans.Data.Common.Constants.SharedContentGuidConfig);
        }

        [HttpGet]
        [Route("/head/[controller]")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]")]
        public virtual  Task<IActionResult> BodyTop()
        {
            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/breadcrumb/[controller]")]
        public virtual IActionResult Breadcrumb(Guid objectId)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewModel.BackLink = GetBackLink(controllerName, objectId);
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual async Task<IActionResult> Body()
        {
            //var sharedContent = await _documentService.GetByIdAsync(_sharedContent, "account").ConfigureAwait(false);
            //ViewModel.SharedContent = sharedContent?.Content;


            try
            {
                var sharedhtml = await sharedContentRedis.GetDataAsync<SharedHtml>("sharedContent/" + SharedContectStaxId);

                ViewModel.SharedContent = sharedhtml.Html;

            }
            catch
            {
                ViewModel.SharedContent = "";
            }
         
            return await Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/bodyfooter/[controller]")]
        public virtual IActionResult BodyFooter()
        {
            return View(ViewModel);
        }
       
        protected IActionResult RedirectTo(string relativeAddress)
        {
            relativeAddress = $"~{ViewModel.CompositeSettings.Path}/" + relativeAddress;
            
            return Redirect(relativeAddress);
        }
        protected async Task<Customer> GetCustomerDetails()
        {
            return await _dssReader.GetCustomerDetails(GetLoggedInUserId());
        }

        protected string GetLoggedInUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;
        }

        protected string GetSessionId()
        {
            var compositeSessionId = Request.CompositeSessionId();
            return compositeSessionId.HasValue ? $"{Request.CompositeSessionId()}|{GetLoggedInUserId()}" : null;
        }

        protected async Task<UserSession> GetUserSession()
        {
            return await GetUserSession(GetSessionId());
        }

        protected async Task ManageSession(Guid customerId, Guid actionPlanId, Guid interactionId, UserSession session = null)
        {
            session ??= await GetUserSession();
            
            if (session == null)
            {
                var interaction =
                    await _dssReader.GetInteractionDetails(customerId.ToString(), interactionId.ToString());
                var adviser = await _dssReader.GetAdviserDetails(interaction.AdviserDetailsId);
                session = new UserSession()
                {
                    Id = GetSessionId(),
                    ActionPlanId = actionPlanId,
                    InteractionId = interactionId,
                    CustomerId = customerId,
                    Interaction = interaction, 
                    Adviser = adviser
                };
                await CreateUserSession(session);

                ViewModel.CustomerId = customerId;
                ViewModel.InteractionId = interactionId;
                ViewModel.ActionPlanId = actionPlanId;
                ViewModel.Interaction = session.Interaction;
                ViewModel.Adviser = session.Adviser;
            }
            else
            {
                interactionId = interactionId == Guid.Empty ? session.InteractionId : interactionId;
                var interaction =
                    await _dssReader.GetInteractionDetails(session.CustomerId.ToString(), interactionId.ToString());
                if (actionPlanId != Guid.Empty && interactionId != Guid.Empty)
                {
                    session.ActionPlanId = actionPlanId;
                    session.InteractionId = interactionId;
                    session.Interaction = interaction;
                }
                await UpdateSession(session);
                ViewModel.CustomerId = session.CustomerId;
                ViewModel.InteractionId = session.InteractionId;
                ViewModel.ActionPlanId = session.ActionPlanId;
                ViewModel.Interaction = session.Interaction;
                ViewModel.Adviser = session.Adviser;
            }
        }


        private string GetBackLink(string controllerName, Guid objectId)
        {
            switch (controllerName)
            {
                case Constants.ChangeGoalDueDateController: 
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, objectId);
                    
                case Constants.ChangeGoalStatusController:
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, objectId); 

                case Constants.ChangeActionDueDateController: 
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path, objectId);
                    
                case Constants.ChangeActionStatusController:
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path,objectId);
                    
                default:
                    return ViewModel.CompositeSettings.Path + "/home";
                    
            }
        }
    }

}

