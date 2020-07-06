using System;
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

        public static string GetViewGoalLink(String CompositePath, Guid ActionPlanId, Guid InteractionId, Guid GoalId)
        {
            var link = $"{CompositePath}/{CompositeViewModel.PageId.ViewGoal}/{ActionPlanId}/{InteractionId}/{GoalId}";
            return link;
        }
        public static string GetViewActionLink(String CompositePath, Guid ActionPlanId, Guid InteractionId, Guid GoalId)
        {
            var link = $"{CompositePath}/{CompositeViewModel.PageId.ViewAction}/{ActionPlanId}/{InteractionId}/{GoalId}";
            return link;
        }

        public static string GetViewActionPlanLink(String CompositePath, Guid ActionPlanId, Guid InteractionId)
        {
            var link = $"{CompositePath}/{CompositeViewModel.PageId.Home}/{ActionPlanId}/{InteractionId}";
            return link;
        }
    }
}
