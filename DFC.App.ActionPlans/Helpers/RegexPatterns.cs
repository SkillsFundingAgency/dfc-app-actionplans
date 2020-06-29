using System;

namespace DFC.App.ActionPlans.Helpers
{
    public static class RegexPatterns
    {
        public const String Day = @"^(0[1-9]|[1-9]|[1-2][0-9]|3[0-1])$";
        public const String Month = @"^(0[1-9]|[1-9]|1[0-2])$";
        public const String Numeric = @"^[0-9]*$";
    }
}
