﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.ViewModels
{  
        public class ViewActionCompositeViewModel:CompositeViewModel
        {
            public ViewActionCompositeViewModel()
                : base(CompositeViewModel.PageId.ViewAction, "View or update action")
            {
            
            }

            public Services.DSS.Models.Action Action{ get; set; }
        }
}
