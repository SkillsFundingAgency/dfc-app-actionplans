using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum ActionStatus
    {
        [Display(Name = "Not Started")]
        NotStarted=1,
        [Display(Name = "In Progress")]
        InProgress=2,  
        [Display(Name = "Completed")]
        Completed,  
        [Display(Name = "No longer applicable")]
        NoLongerApplicable=99
    }
}
