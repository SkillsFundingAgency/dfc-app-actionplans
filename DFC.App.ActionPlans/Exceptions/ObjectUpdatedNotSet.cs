using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class ObjectUpdatedNotSet : System.Exception, ISerializable
    {
        public ObjectUpdatedNotSet(string message)
            : base(message)
        {

        }
    }
}