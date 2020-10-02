using System;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ChangeGoalCompositeViewModel : CompositeViewModel
    {
        public ChangeGoalCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "")
        {
            
        }

        public Goal Goal{ get; set; }
        
        public String ErrorMessage { get; set; } = "";

        public SplitDate DateGoalShouldBeCompletedBy { get; set; }
    }
}
