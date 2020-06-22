using System;

namespace DFC.App.ActionPlans.ViewModels
{
    public class BreadCrumbViewModel :CompositeViewModel
    {
        public String HomeUrl { get; set; }

        public BreadCrumbViewModel()
            : base(PageId.Home, "Breadcrumb")
        {
            
        }
    }
}
