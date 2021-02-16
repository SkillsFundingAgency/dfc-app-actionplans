using System;
using DFC.App.ActionPlans.ViewModels;

namespace DFC.App.ActionPlans.Helpers
{
    public static class Urls
    {
        public static string GetUpdateConfirmationUrl(Guid ObjectId, int ObjectUpdated, int PropertyUpdated)
        {
            var url = $"{CompositeViewModel.PageId.UpdateConfirmation}?objectId={ObjectId}&objectUpdated={ObjectUpdated}&propertyUpdated={PropertyUpdated}";
            return url;
        }

        public static string GetViewGoalUrl(String CompositePath, Guid GoalId)
        {
            var url = $"{CompositePath}/{CompositeViewModel.PageId.ViewGoal}?goalId={GoalId}";
            return url;
        }
        public static string GetViewActionUrl(String CompositePath, Guid ActionId)
        {
            var url = $"{CompositePath}/{CompositeViewModel.PageId.ViewAction}?actionId={ActionId}";
            return url;
        }

        public static string GetChangeUrl(String CompositePath, CompositeViewModel.PageId pageId, Guid ObjectId, string paramName)
        {
            var url = $"{CompositePath}/{pageId}?{paramName}={ObjectId}";
            return url;
        }

        public static string GetViewActionPlanUrl(string CompositePath)
        {
            var url = $"{CompositePath}/home";
            return url;
        }
    }
}
