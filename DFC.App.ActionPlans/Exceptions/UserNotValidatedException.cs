using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
