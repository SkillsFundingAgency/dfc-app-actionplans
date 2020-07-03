using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class ObjectUpdatedNotSetException : System.Exception, ISerializable
    {
        public ObjectUpdatedNotSetException(string message)
            : base(message)
        {

        }
    }
}