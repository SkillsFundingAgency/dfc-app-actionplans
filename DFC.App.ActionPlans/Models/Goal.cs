using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.Models
{
    public class Goal
    {
        public string GoalId { get; set; }
        public string CustomerId { get; set; }
        public string ActionPlanId { get; set; }
        public string SubcontractorId { get; set; }
        public DateTime DateGoalCaptured { get; set; }
        public DateTime DateGoalShouldBeCompletedBy { get; set; }
        public DateTime DateGoalAchieved { get; set; }
        public string GoalSummary { get; set; }
        public int GoalType { get; set; }
        public int GoalStatus { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
