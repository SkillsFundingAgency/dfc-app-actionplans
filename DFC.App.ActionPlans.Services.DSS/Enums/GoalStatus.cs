using System.ComponentModel.DataAnnotations;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum GoalStatus
    {
        [Display(Name = "In progress", Order=2)]
        InProgress=1,
        [Display(Name = "Achieved", Order=1)]
        Achieved,  
        [Display(Name = "No longer relevant", Order=3)]
        NoLongerRelevant=99
    }
}
