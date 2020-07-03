using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class PropertyUpdatedNotSet : System.Exception, ISerializable
    {
        public PropertyUpdatedNotSet(string message)
            : base(message)
        {

        }
    }
}
