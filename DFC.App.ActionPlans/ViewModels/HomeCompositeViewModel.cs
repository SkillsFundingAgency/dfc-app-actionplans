using System.Collections.Generic;
using DFC.App.ActionPlans.Services.DSS.Models;
using Action = DFC.App.ActionPlans.Services.DSS.Models.Action;

namespace DFC.App.ActionPlans.ViewModels
{
    public class HomeCompositeViewModel : CompositeViewModel
    {
        
        public HomeCompositeViewModel()
            : base(PageId.Home, "Your action plan")
        {
            
        }
        public List<Goal> Goals { get; set; }
        public List<Action> Actions { get; set;}
        public Session LatestSession { get; set;}
        public ActionPlan ActionPlan { get; set;}
       
        
    }
}
