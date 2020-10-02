using System;
using DFC.App.ActionPlans.Services.DSS.Models;
using Newtonsoft.Json;

namespace DFC.App.ActionPlans.Models
{
    public class UserSession
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid ActionPlanId { get; set; }
        public Interaction Interaction { get; set; }
        public Adviser Adviser { get; set; }

    }
}
