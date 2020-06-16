using System.Collections.Generic;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class HomeCompositeViewModel : CompositeViewModel
    {
        
        public HomeCompositeViewModel()
            : base(PageId.Home, "Your action plan")
        {
            
        }
        public List<Goal> goals { get; set; }
        public List<Action> actions { get; set;}
        public Session latestSession { get; set;}
        public Interaction interaction { get; set;}
        public Adviser adviser { get; set;}

    }
}
