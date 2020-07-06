using System;
using DFC.App.ActionPlans.Services.DSS.Enums;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class UpdateAction
    {
        public Guid ActionPlanId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid ActionId { get; set; }
        public ActionStatus ActionStatus { get; set; }
        public DateTime DateActionShouldBeCompletedBy { get; set; }
    }
}
