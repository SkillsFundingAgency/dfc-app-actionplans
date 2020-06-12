using System;

namespace DFC.App.ActionPlans.Services.DSS.Models
{

    public class Session
    {
        public string SessionId { get; set; }
        public string CustomerId { get; set; }
        public string InteractionId { get; set; }
        public DateTime DateandTimeOfSession { get; set; }
        public string VenuePostCode { get; set; }
        public string SessionAttended { get; set; }
        public string ReasonForNonAttendance { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedTouchpointId { get; set; }
        public string SubcontractorId { get; set; }
    }

}
