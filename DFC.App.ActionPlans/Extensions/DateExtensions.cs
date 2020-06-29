using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.Extensions
{
    public static class DateExtensions
    {
        public static string DateOnly(this DateTime date)
        {
            
            return date.ToString("dd MMMM yyyy");
        }
    }
}
