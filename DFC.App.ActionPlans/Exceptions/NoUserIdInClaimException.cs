﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.Exceptions
{
    [Serializable]
    public class NoUserIdInClaimException : System.Exception, ISerializable
    {
        public NoUserIdInClaimException(string message)
            : base(message)
        {

        }
    }
}
