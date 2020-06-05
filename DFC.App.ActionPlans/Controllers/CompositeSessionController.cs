using System.Collections.Generic;
using System.Linq;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DFC.App.Account.Services.DSS.Interfaces;
using DFC.App.ActionPlans.Exceptions;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace Dfc.App.ActionPlans.Controllers
{
  
    /// <summary>
    /// Adds default Composite UI endpoints and routing logic to the base session controller.
    /// </summary>
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
        protected virtual IActionResult RedirectWithError(string controller, string parameters = "")
        {

            if (!string.IsNullOrEmpty(parameters))
            {
                 parameters = $"&{parameters}";
            }

            return RedirectTo($"{controller}?errors=true{parameters}");
        }

        protected bool HasErrors()
        {
            var errorsString = Request.Query["errors"];
            var parsed = bool.TryParse(errorsString, out var error);
            return parsed && error;
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
    }

}

