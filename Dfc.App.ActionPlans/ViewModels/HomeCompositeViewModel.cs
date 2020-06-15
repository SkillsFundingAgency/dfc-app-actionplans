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

        public IList<Goal> goals { get; set; }
        public IList<Action> actions { get; set;}

        

    }
}
