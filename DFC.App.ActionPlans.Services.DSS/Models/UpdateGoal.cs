using System;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class UpdateGoal
    {
        public Guid ActionPlanId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid GoalId { get; set; }
        public DateTime DateGoalShouldBeCompletedBy { get; set; }
    }
}
