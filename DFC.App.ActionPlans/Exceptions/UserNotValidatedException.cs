using System;
using System.Runtime.Serialization;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class UserNotValidatedException :  System.Exception, ISerializable
    {
        public UserNotValidatedException(string message)
            : base(message)
        {

        }

    }
}
