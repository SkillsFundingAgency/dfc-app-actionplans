﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DFC.App.ActionPlans.Helpers;

namespace DFC.App.ActionPlans.ViewModels
{
    public class ChangeGoalDueDateCompositeViewModel : CompositeViewModel
    {
        public ChangeGoalDueDateCompositeViewModel()
            : base(CompositeViewModel.PageId.ViewGoal, "Change Goal due date")
        {
            
        }

        public Guid ActionPlanId { get; set; } 
        public Guid InteractionId { get; set;}

        [RegularExpression(RegexPatterns.Day, ErrorMessage = "Day must be a number between 1 and 31")]
        [Required]
        public string Day { get; set; }
        [Required]
        [RegularExpression(RegexPatterns.Month, ErrorMessage = "Month must be a number between 1 and 12")]
        public string Month { get; set; }
        [Required]
        [RegularExpression(RegexPatterns.Numeric, ErrorMessage = "Year must be a number")]
        public string Year { get; set; }

        public Services.DSS.Models.Goal Goal{ get; set; }
        
        public String ErrorMessage { get; set; } = "Entered due date is in the past or invalid";
    }
}
