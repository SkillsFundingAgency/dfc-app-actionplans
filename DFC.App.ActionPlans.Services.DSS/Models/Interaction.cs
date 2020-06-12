using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class Interaction
    {
        public string InteractionId { get; set; }
        public string CustomerId { get; set; }
        public string TouchpointId { get; set; }
        public string AdviserDetailsId { get; set; }
        public DateTime DateandTimeOfInteraction { get; set; }
        public string Channel { get; set; }
        public string InteractionType { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
    }
}