using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ViewGoalCompositeViewModel:CompositeViewModel
    {
        public ViewGoalCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "View or update goal")
        {
            
        }

        public Goal Goal { get; set; }
    }
}
