using System;
using Newtonsoft.Json;

namespace DFC.App.ActionPlans.Models
{
    public class UserSession
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("partitionKey")]
        public Guid PartitionKey { get; set; }
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid ActionPlanId { get; set; }
    }
}
