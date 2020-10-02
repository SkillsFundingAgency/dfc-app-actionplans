using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class PropertyUpdatedNotSetException : System.Exception, ISerializable
    {
        public PropertyUpdatedNotSetException(string message)
            : base(message)
        {

        }
    }
}
