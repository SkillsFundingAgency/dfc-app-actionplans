﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfc.App.ActionPlans.Controllers;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Interfaces;
using DFC.App.ActionPlans.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DFC.App.ActionPlans.Controllers
{
    public class ViewGoalController : CompositeSessionController<ViewGoalCompositeViewModel>
    {
        private IDssReader _dssReader;
        
        public ViewGoalController(ILogger<HomeController> logger, IOptions<CompositeSettings> compositeSettings, IDssReader dssReader)
            :base(compositeSettings, dssReader)
        {
            _dssReader = dssReader;
        }
        
    }
}
