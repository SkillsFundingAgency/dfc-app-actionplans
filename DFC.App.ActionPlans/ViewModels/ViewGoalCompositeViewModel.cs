using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.ViewModels;

namespace DFC.App.ActionPlans.Controllers
{
    public class ViewGoalCompositeViewModel:CompositeViewModel
    {
        public ViewGoalCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "View or update goal")
        {
            
        }
    }
}
