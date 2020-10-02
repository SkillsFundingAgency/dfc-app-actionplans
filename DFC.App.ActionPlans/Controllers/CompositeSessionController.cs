using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Constants;
using DFC.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;

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
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, ICosmosService cosmosServiceService)
            : base(cosmosServiceService)        
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };  
            _dssReader = dssReader;
        }

        [HttpGet]
        [Route("/head/[controller]/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public virtual IActionResult Head(Guid actionPlanId, Guid interactionId, Guid objId, int objectUpdated, int propertyUpdated)
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public virtual  Task<IActionResult> BodyTop()
        {
            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/breadcrumb/[controller]/{actionPlanId?}/{interactionId?}/{objectId?}/{objupdated?}/{itemupdated?}")]
        public virtual IActionResult Breadcrumb(Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ViewModel.BackLink = GetBackLink(controllerName,actionPlanId, interactionId, objectId);
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual Task<IActionResult> Body()
        {

            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/bodyfooter/[controller]/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
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
            
            var userId = User.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (userId == null)
            {
                throw new NoUserIdInClaimException("Unable to locate userID");
            }

            return await _dssReader.GetCustomerDetails(userId);
        }


        protected async Task LoadData(Guid customerId, Guid actionPlanId, Guid interactionId)
        {
            
            var userSession = await GetUserSession(GetSessionId(customerId,actionPlanId,interactionId),"");
           
            if (userSession == null)
            {
                var interaction =
                    await _dssReader.GetInteractionDetails(customerId.ToString(), interactionId.ToString());
                var adviser = await _dssReader.GetAdviserDetails(interaction.AdviserDetailsId);
                userSession = new UserSession()
                {
                    Id = GetSessionId(customerId,actionPlanId,interactionId),
                    ActionPlanId = actionPlanId,
                    InteractionId = interactionId,
                    CustomerId = customerId,
                    Interaction = interaction, 
                    Adviser = adviser
                };
                await CreateUserSession(userSession);
            }

            ViewModel.CustomerId = customerId;
            ViewModel.InteractionId = interactionId;
            ViewModel.ActionPlanId = actionPlanId;
            ViewModel.Interaction = userSession.Interaction;
            ViewModel.Adviser = userSession.Adviser;
        }

        private string GetSessionId(Guid customerId, Guid actionPlanId, Guid interactionId)
        {
            return customerId + "+" + actionPlanId + "+" + interactionId;
        }

        private string GetBackLink(string controllerName, Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            switch (controllerName)
            {
                case Constants.ChangeGoalDueDateController: 
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                case Constants.ChangeGoalStatusController:
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId); 

                case Constants.ChangeActionDueDateController: 
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                case Constants.ChangeActionStatusController:
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                default:
                    return Urls.GetViewActionPlanUrl(ViewModel.CompositeSettings.Path, actionPlanId, interactionId);
                    
            }
        }
    }

}

