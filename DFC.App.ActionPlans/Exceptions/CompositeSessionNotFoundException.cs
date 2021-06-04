using System;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class CompositeSessionNotFoundException : Exception
    {
        public CompositeSessionNotFoundException(string message)
            : base(message)
        {

        }
    }
}
