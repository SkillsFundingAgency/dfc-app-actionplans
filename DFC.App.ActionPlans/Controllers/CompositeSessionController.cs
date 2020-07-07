using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Constants;
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
    public abstract class CompositeSessionController<TViewModel>:Controller where TViewModel : CompositeViewModel, new()
    {
        private readonly IDssReader _dssReader;
        protected TViewModel ViewModel { get; }
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IDssReader dssReader)
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };  
            _dssReader = dssReader;
        }

        [HttpGet]
        [Route("/head/[controller]/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]/{actionPlanId?}/{interactionId?}/{docId?}/{objupdated?}/{itemupdated?}")]
        public virtual async Task<IActionResult> BodyTop()
        {
            if (User.Identity.IsAuthenticated)
            {
                var customer = await GetCustomerDetails();
                ViewModel.Name = $"{customer.GivenName} {customer.FamilyName}";
            }

            return View(ViewModel);
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
            /*
             TODO: Enable Autorization
            var userId = User.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (userId == null)
            {
                throw new NoUserIdInClaimException("Unable to locate userID");
            }

            return await _dssReader.GetCustomerDetails(userId);
            */
            return new Customer(){CustomerId = new Guid("53f904b3-77c8-4c94-9a15-c259b518336c"),FamilyName = "Family",GivenName = "Given"};
        }


        protected async Task LoadData(Guid customerId, Guid actionPlanId, Guid interactionId)
        {
            List<Session> sessions;
            ViewModel.CustomerId = customerId;
            ViewModel.InteractionId = interactionId;
            ViewModel.ActionPlanId = actionPlanId;
            ViewModel.Interaction = await _dssReader.GetInteractionDetails(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            ViewModel.Adviser = await _dssReader.GetAdviserDetails(ViewModel.Interaction.AdviserDetailsId);
            sessions = await _dssReader.GetSessions(ViewModel.CustomerId.ToString(), ViewModel.InteractionId.ToString());
            ViewModel.LatestSession = sessions.OrderByDescending(s => s.DateandTimeOfSession).First();
        }

        private string GetBackLink(String controllerName, Guid actionPlanId, Guid interactionId, Guid objectId)
        {
            switch (controllerName)
            {
                case Constants.ChangeGoalDueDateController: 
                    return @Links.GetViewGoalLink(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                case Constants.ChangeGoalStatusController:
                    return @Links.GetViewGoalLink(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId); 

                case Constants.ChangeActionDueDateController: 
                    return @Links.GetViewActionLink(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                case Constants.ChangeActionStatusController:
                    return @Links.GetViewActionLink(ViewModel.CompositeSettings.Path, actionPlanId, interactionId, objectId);
                    
                default:
                    return @Links.GetViewActionPlanLink(ViewModel.CompositeSettings.Path, actionPlanId, interactionId);;
                    
            }

            
        }
    }

}

