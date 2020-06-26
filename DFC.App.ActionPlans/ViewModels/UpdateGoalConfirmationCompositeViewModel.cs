using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.ViewModels
{
    public class UpdateGoalConfirmationCompositeViewModel : CompositeViewModel 
    {
        public UpdateGoalConfirmationCompositeViewModel() : base(PageId.Error, "Goal Updated")
        {
        }
    }
}
