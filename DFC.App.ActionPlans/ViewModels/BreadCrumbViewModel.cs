using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
