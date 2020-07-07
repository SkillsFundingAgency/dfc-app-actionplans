using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.ActionPlans.Models
{
    public class UserSession
    {
        public Guid CustomerId { get; set; }
        public Guid InteractionId { get; set; }
        public Guid ActionPlanId { get; set; }

    }
}
