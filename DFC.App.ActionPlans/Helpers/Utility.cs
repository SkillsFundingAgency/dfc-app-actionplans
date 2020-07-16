using System;
using DFC.App.ActionPlans.Services.DSS.Enums;
using DFC.Personalisation.Common.Extensions;

namespace DFC.App.ActionPlans.Helpers
{
    public static class Utility
    {
        public static string GetActionHelperText(SignpostedToCategory? signpostedToCategory, string signpostedTo)
        {
            var returnValue = "";
            if (signpostedToCategory != null)
            {
                returnValue = signpostedToCategory.GetDisplayName() + " - ";
            }

            if (Uri.IsWellFormedUriString(signpostedTo, UriKind.RelativeOrAbsolute))
            {
                returnValue += $"<a id='helplink' href='{signpostedTo}' target='_blank'>{signpostedTo}</a>";
                    
            }
            else
            {
                returnValue += signpostedTo;
            }

            return returnValue;
        }
    }
}
