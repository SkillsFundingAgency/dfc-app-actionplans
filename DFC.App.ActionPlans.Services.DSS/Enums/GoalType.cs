using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum GoalType
    {
        [Display(Name = "Skills")]
        Skills,
        [Display(Name = "Work")]
        Work,
        [Display(Name = "Learning")]
        Learning,
        [Display(Name = "Other")]
        Other=99
    }
}
