using System;
using DFC.App.ActionPlans.ViewModels;

namespace DFC.App.ActionPlans.Helpers
{
    public static class Urls
    {
        public static string GetUpdateConfirmationUrl(Guid ActionPlanId, Guid InteractionId, Guid ObjectId, int ObjectUpdated, int PropertyUpdated)
        {
            var url = $"{CompositeViewModel.PageId.UpdateConfirmation}/{ActionPlanId}/{InteractionId}/{ObjectId}/{ObjectUpdated}/{PropertyUpdated}";
            return url;
        }

        public static string GetViewGoalUrl(String CompositePath, Guid ActionPlanId, Guid InteractionId, Guid GoalId)
        {
            var url = $"{CompositePath}/{CompositeViewModel.PageId.ViewGoal}/{ActionPlanId}/{InteractionId}/{GoalId}";
            return url;
        }
        public static string GetViewActionUrl(String CompositePath, Guid ActionPlanId, Guid InteractionId, Guid GoalId)
        {
            var url = $"{CompositePath}/{CompositeViewModel.PageId.ViewAction}/{ActionPlanId}/{InteractionId}/{GoalId}";
            return url;
        }

        public static string GetViewActionPlanUrl(String CompositePath, Guid ActionPlanId, Guid InteractionId)
        {
            var url = $"{CompositePath}/{ActionPlanId}/{InteractionId}";
            return url;
        }

        public static string GetChangeUrl(String CompositePath, CompositeViewModel.PageId pageId, Guid ActionPlanId, Guid InteractionId, Guid ObjectId)
        {
            var url = $"{CompositePath}/{pageId}/{ActionPlanId}/{InteractionId}/{ObjectId}";
            return url;
        }
    }
}
