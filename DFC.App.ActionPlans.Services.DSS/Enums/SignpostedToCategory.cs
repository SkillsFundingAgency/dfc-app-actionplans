using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{
    public enum SignpostedToCategory
    {
        [Display(Name = "Charity National")] CharityNational = 1,
        [Display(Name = "Charity Local")] CharityLocal = 2,
        [Display(Name = "Training Provider")] TrainingProvider = 3,
        [Display(Name = "College")] College = 4,

        [Display(Name = "National Retraining Scheme")]
        NationalRetrainingScheme = 5,

        [Display(Name = "Apprenticeship Service")]
        ApprenticeshipService = 6,

        [Display(Name = "Specialist Organisation")]
        SpecialistOrganisation = 7,
        [Display(Name = "JCP")] JCP = 8,
        [Display(Name = "Employer")] Employer = 9,
        [Display(Name = "Other")] Other = 99
    }
}