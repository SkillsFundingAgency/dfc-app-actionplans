using System;
using DFC.App.ActionPlans.Services.DSS.Enums;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class UpdateGoal
    {
        public Guid ActionPlanId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid GoalId { get; set; }
        public GoalStatus GoalStatus { get; set; }
        public DateTime DateGoalShouldBeCompletedBy { get; set; }
    }
}
