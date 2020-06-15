using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Exceptions;
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
        [Route("/head/[controller]/{id?}")]
        public virtual IActionResult Head()
        {
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/bodytop/[controller]/{id?}")]
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
        [Route("/breadcrumb/[controller]/{id?}")]
        public virtual IActionResult Breadcrumb()
        {
            var pagesThatDontNeedBreadCrumbs = new List<CompositeViewModel.PageId>
            {
                CompositeViewModel.PageId.Home
            };

            ViewModel.ShowBreadCrumb = !pagesThatDontNeedBreadCrumbs.Contains(ViewModel.Id);
            
            return View(ViewModel);
        }

        [HttpGet]
        [Route("/body/[controller]/{id?}")]
        public virtual Task<IActionResult> Body()
        {
            return Task.FromResult<IActionResult>(View(ViewModel));
        }

        [HttpGet]
        [Route("/bodyfooter/[controller]/{id?}")]
        public virtual IActionResult BodyFooter()
        {
            return View(ViewModel);
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
            return new Customer(){CustomerId = new Guid("53f904b3-77c8-4c94-9a15-c259b518336c")};
        }
    }

}

