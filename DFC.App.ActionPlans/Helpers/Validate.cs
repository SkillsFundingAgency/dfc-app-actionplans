using System;
using System.Globalization;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.Helpers
{
    public static  class Validate
    {
        public static bool CheckValidSplitDate(SplitDate splitDate, out DateTime dateValue)
        {
            CultureInfo enGb = new CultureInfo("en-GB");
            if (DateTime.TryParseExact($"{splitDate.Day}/{splitDate.Month}/{splitDate.Year}","dd/MM/yyyy", enGb, DateTimeStyles.AdjustToUniversal,out dateValue))
                return true;
            return false;

        }

        public static bool CheckValidDueDate(SplitDate splitDate, out DateTime dateValue)
        {
            bool isValidSplitDate = CheckValidSplitDate(splitDate, out dateValue);
            if (isValidSplitDate && dateValue >= DateTime.Now.Date)
                return true;
            return false;
        }
    }
}
