using System;
using System.Collections.Generic;
using DFC.App.ActionPlans.Models;
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
        public Interaction Interaction { get; set;}
        public Adviser Adviser { get; set;}
        public Guid CustomerId { get; set;}
        public Guid ActionPlanId { get; set;}
        public Guid InteractionId { get; set;}
        
    }
}
