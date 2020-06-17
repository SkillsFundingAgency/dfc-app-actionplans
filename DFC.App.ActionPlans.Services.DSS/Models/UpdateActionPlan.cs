using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class UpdateActionPlan
    {
        public Guid ActionPlanId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public DateTime DateActionPlanAcknowledged { get; set; }
    }
}
