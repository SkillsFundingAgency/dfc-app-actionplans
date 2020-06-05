using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DFC.App.ActionPlans.Services.DSS.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        
    }

}
