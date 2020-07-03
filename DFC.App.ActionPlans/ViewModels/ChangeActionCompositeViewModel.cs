using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ChangeActionCompositeViewModel  : CompositeViewModel
    {
        public ChangeActionCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "")
        {
            
        }

        public Services.DSS.Models.Action Action{ get; set; }
        
        public String ErrorMessage { get; set; } = "";

        public SplitDate DateGoalShouldBeCompletedBy { get; set; }
    }
}
