namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class DssSettings
    {
        public string CustomerApiUrl { get; set; }
        public string CustomerApiVersion { get; set; }

        public string SessionApiUrl { get; set; }
        public string SessionApiVersion { get; set; }
        
        public string AdviserDetailsApiUrl { get; set; }
        public string AdviserDetailsApiVersion { get; set; }

        public string InteractionsApiUrl { get; set; }
        
        public string GoalsApiUrl { get; set; }
        public string GoalsApiVersion { get; set; }

        public string ActionsApiUrl { get; set; }
        public string ActionsApiVersion { get; set; }
        
        public string ActionPlansApiUrl { get; set; }
        public string ActionPlansApiVersion { get; set; }

        public string TouchpointId { get; set; }
        public string ApiKey { get; set; }
    }
}
