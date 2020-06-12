using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class Action
    {
        public string ActionId { get; set; }
        public string CustomerId { get; set; }
        public string ActionPlanId { get; set; }
        public DateTime DateActionAgreed { get; set; }
        public DateTime DateActionAimsToBeCompletedBy { get; set; }
        public DateTime DateActionActuallyCompleted { get; set; }
        public string ActionSummary { get; set; }
        public string SignpostedTo { get; set; }
        public object SignpostedToCategory { get; set; }
        public int ActionType { get; set; }
        public int ActionStatus { get; set; }
        public int PersonResponsible { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
    }
}