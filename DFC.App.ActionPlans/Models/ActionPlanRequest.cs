using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ActionPlans.Models
{
    [ExcludeFromCodeCoverage]
    public class ActionPlanRequest
    {
        public Guid ActionPlanId { get; set; }
        public Guid InteractionId { get; set; }
    }
}
