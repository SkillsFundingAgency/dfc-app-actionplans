using System;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.Extensions
{
    public static class SplitDateExtentions
    {
        public static bool isEmpty(this SplitDate splitDate)
        {
            if (String.IsNullOrWhiteSpace(splitDate.Day) || String.IsNullOrWhiteSpace(splitDate.Month) ||
                String.IsNullOrWhiteSpace(splitDate.Year))
            {
                return true;
            }

            return false;
        }
    }
}
