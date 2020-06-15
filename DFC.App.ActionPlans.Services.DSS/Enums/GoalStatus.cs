using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum GoalStatus
    {
        [Display(Name = "In progress")]
        InProgress=1,
        [Display(Name = "Achieved")]
        Achieved,  
        [Display(Name = "No longer relevant")]
        NoLongerRelevant=99
    }
}
