﻿using DFC.App.ActionPlans.Services.DSS.Models;

namespace DFC.App.ActionPlans.ViewModels
{
    public class UpdateGoalConfirmationCompositeViewModel : CompositeViewModel 
    {
        public UpdateGoalConfirmationCompositeViewModel() : base(PageId.Error, "Goal Updated")
        {
        }

        public Goal Goal { get; set; }
    }
}
