using System.ComponentModel.DataAnnotations;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum GoalType
    {
        [Display(Name = "Skills")]
        Skills=1,
        [Display(Name = "Work")]
        Work=2,
        [Display(Name = "Learning")]
        Learning=3,
        [Display(Name = "Other")]
        Other=99
    }
}
