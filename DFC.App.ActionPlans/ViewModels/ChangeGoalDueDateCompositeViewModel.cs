using System;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ChangeGoalDueDateCompositeViewModel : CompositeViewModel
    {
        public ChangeGoalDueDateCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "Change Goal due date")
        {
            
        }

        public Goal Goal{ get; set; }
        
        public String ErrorMessage { get; set; } = "Entered due date is in the past or invalid";

        public SplitDate DateGoalShouldBeCompletedBy { get; set; }
    }
}
