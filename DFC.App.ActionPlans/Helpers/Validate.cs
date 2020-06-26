using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Models;

namespace DFC.App.ActionPlans.Helpers
{
    public static  class Validate
    {
        public static bool CheckValidSplitDate(SplitDate splitDate, out DateTime dateValue)
        {
            if (DateTime.TryParse($"{splitDate.Day}/{splitDate.Month}/{splitDate.Year}", out dateValue))
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
