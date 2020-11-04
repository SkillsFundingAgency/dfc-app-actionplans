using System;

namespace DFC.App.ActionPlans.Models
{
    public class ActionPlanRequest
    {
        public Guid ActionPlanId { get; set; }
        public Guid InteractionId { get; set; }
    }
}
