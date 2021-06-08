using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]

    public class CompositeSessionNotFoundException : System.Exception, ISerializable
    {
        public CompositeSessionNotFoundException(string message)
            : base(message)
        {

        }
    }
}
