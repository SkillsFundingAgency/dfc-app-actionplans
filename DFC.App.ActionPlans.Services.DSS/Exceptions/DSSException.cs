using System;

namespace DFC.App.ActionPlans.Services.DSS
{
    public class DssException : Exception
    {
        public DssException(string message)
            :base(message)
        { }

    }
}
