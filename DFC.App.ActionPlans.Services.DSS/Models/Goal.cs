using System;
using DFC.App.ActionPlans.Services.DSS.Enums;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class Goal
    {
        public string GoalId { get; set; }
        public string CustomerId { get; set; }
        public string ActionPlanId { get; set; }
        public string SubcontractorId { get; set; }
        public DateTime DateGoalCaptured { get; set; }
        public DateTime DateGoalShouldBeCompletedBy { get; set; }
        public DateTime? DateGoalAchieved { get; set; }
        public string GoalSummary { get; set; }
        public GoalType GoalType { get; set; }
        public GoalStatus GoalStatus { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
