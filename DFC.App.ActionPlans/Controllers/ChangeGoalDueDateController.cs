using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class ChangeGoalDueDateController: CompositeSessionController<ChangeGoalDueDateCompositeViewModel>
    {
        
            private readonly IDssReader _dssReader;
        
            public ChangeGoalDueDateController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader)
                :base(compositeSettings, dssReader)
            {
                _dssReader = dssReader;
            }
        
            //  [Authorize]
            [Route("/body/change-goal-due-date/{actionPlanId}/{interactionId}/{goalId}")]
            [HttpGet]
            public async  Task<IActionResult> Body(Guid actionPlanId, Guid interactionId, Guid goalId)
            {
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, actionPlanId, interactionId);
                return await base.Body();
            }

            [Route("/body/change-goal-due-date")]
            [HttpPost]
            public async  Task<IActionResult> Body(ChangeGoalDueDateCompositeViewModel model, IFormCollection formCollection)
            {

                ViewModel.ActionPlanId = model.ActionPlanId;
                ViewModel.InteractionId = model.InteractionId;
                ViewModel.Day = model.Day;
                ViewModel.Month = model.Month;
                ViewModel.Year = model.Year;

                if (ModelState.IsValid)
                {
                    DateTime dateValue;
                    if (DateTime.TryParse($"{model.Day}/{model.Month}/{model.Year}",out dateValue))
                    {
                        if (dateValue >= DateTime.Now.Date)
                        {
                            //Update
                        }
                    }
                }

                ModelState.Clear(); //Remove model binding errors as we will check if the date is valid  or not.
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
                   
                var customer = await GetCustomerDetails();
                await LoadData(customer.CustomerId, model.ActionPlanId, model.InteractionId);
                return await base.Body();
            }
    }
}
