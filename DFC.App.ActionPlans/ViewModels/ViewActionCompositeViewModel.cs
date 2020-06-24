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
