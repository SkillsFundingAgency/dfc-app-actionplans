using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ErrorCompositeViewModel : CompositeViewModel
    {
        public ErrorCompositeViewModel() : base(PageId.Error, "Service Error")
        {
        }
    }
}
