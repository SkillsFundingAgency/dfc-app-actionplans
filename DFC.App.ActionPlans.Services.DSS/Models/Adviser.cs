using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class Adviser
    {
        public string AdviserDetailId { get; set; }
        public string AdviserName { get; set; }
        public string AdviserEmailAddress { get; set; }
        public string AdviserContactNumber { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public string SubcontractorId { get; set; }
    }
}
