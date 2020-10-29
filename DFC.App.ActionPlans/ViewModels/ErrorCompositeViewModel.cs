namespace DFC.App.ActionPlans.ViewModels
{
    public class ErrorCompositeViewModel : CompositeViewModel
    {
        public ErrorCompositeViewModel() : base(PageId.Error, "Service Error")
        {

        }

        public bool ShowEnhancedLog { get; set; }
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
