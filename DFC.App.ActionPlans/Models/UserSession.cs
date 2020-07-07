using System;

namespace DFC.App.ActionPlans.Models
{
    public class UserSession
    {
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid ActionPlanId { get; set; }

    }
}
