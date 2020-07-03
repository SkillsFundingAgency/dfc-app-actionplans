using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.ViewModels;

namespace DFC.App.ActionPlans.Helpers
{
    public static class Links
    {
        public static string GetUpdateConfirmationLink(Guid ActionPlanId, Guid InteractionId, Guid ObjectId, int ObjectUpdated, int PropertyUpdated)
        {
            var link = $"{CompositeViewModel.PageId.UpdateConfirmation}/{ActionPlanId}/{InteractionId}/{ObjectId}/{ObjectUpdated}/{PropertyUpdated}";
            return link;
        }
    }
}
