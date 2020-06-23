using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.ViewModels
{  
        public class ViewActionCompositeViewModel:CompositeViewModel
        {
            public ViewActionCompositeViewModel()
                : base(CompositeViewModel.PageId.ViewGoal, "View or update goal")
            {
            
            }

            public Services.DSS.Models.Action Action{ get; set; }
        }
}
