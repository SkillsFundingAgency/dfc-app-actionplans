namespace DFC.App.ActionPlans.Models
{
    public class CompositeSettings
    {
        // Path that is registered for this application in the Cosmos 'paths' collection
        public string Path { get; set; }
        public string Cdn { get; set; }
        public bool EnhancedError { get; set; }
    }
}
