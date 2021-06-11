﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Cosmos.Interfaces;
using DFC.APP.ActionPlans.Data.Models;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Helpers;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using Microsoft.Extensions.Configuration;
using DFC.APP.ActionPlans.Data;

namespace DFC.App.ActionPlans.Controllers
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
        private readonly IDocumentService<CmsApiSharedContentModel> _documentService;
        private readonly Guid _sharedContent;
        protected CompositeSessionController(IOptions<CompositeSettings> compositeSettings, IDssReader dssReader, ICosmosService cosmosServiceService, IDocumentService<CmsApiSharedContentModel> documentService, IConfiguration config)
            : base(cosmosServiceService)        
        {
            ViewModel = new TViewModel()
            {
                CompositeSettings = compositeSettings.Value,
            };  
            _dssReader = dssReader;
            _documentService = documentService;
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
            var sharedContent = await _documentService.GetByIdAsync(_sharedContent, "account").ConfigureAwait(false);
            ViewModel.SharedContent = sharedContent?.Content;
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
            var userId = User.Claims.FirstOrDefault(x => x.Type == "CustomerId")?.Value;

            if (userId == null)
            {
                throw new NoUserIdInClaimException("Unable to locate userID");
            }

            return userId;
        }

        protected string GetSessionId()
        {
            var compositeSessionId = Request.CompositeSessionId();

            if (compositeSessionId == null)
            {
                throw new CompositeSessionNotFoundException("Composite Session is null");
            }

            return $"{Request.CompositeSessionId()}|{GetLoggedInUserId()}";
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
                case Constants.Constants.ChangeGoalDueDateController: 
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, objectId);
                    
                case Constants.Constants.ChangeGoalStatusController:
                    return Urls.GetViewGoalUrl(ViewModel.CompositeSettings.Path, objectId); 

                case Constants.Constants.ChangeActionDueDateController: 
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path, objectId);
                    
                case Constants.Constants.ChangeActionStatusController:
                    return Urls.GetViewActionUrl(ViewModel.CompositeSettings.Path,objectId);
                    
                default:
                    return ViewModel.CompositeSettings.Path + "/home";
                    
            }
        }
    }

}

