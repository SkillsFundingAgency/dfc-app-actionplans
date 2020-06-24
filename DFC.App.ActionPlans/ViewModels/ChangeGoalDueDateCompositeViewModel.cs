using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ChangeGoalDueDateCompositeViewModel : CompositeViewModel
    {
        public ChangeGoalDueDateCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "Change Goal due date")
        {
            
        }

        public Services.DSS.Models.Goal Goal{ get; set; }
    }
}
