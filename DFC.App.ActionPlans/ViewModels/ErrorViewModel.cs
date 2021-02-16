using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ActionPlans.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ErrorViewModel
    {
        public bool ShowEnhancedLog { get; set; }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
