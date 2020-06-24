using System.ComponentModel.DataAnnotations;

namespace DFC.App.ActionPlans.Services.DSS.Enums
{

    public enum ActionType
        {
            [Display(Name = "Skills Health Check")]
            SkillsHealthCheck = 1,
            [Display(Name = "Create or update CV")]
            CreateOrUpdateCv = 2,
            [Display(Name = "Interview Skills Workshop")]
            InterviewSkillsWorkshop = 3,
            [Display(Name = "Search For Vacancy")]
            SearchForVacancy = 4,
            [Display(Name = "Enrol On A Course")]
            EnrolOnACourse = 5,
            [Display(Name = "Careers Management Workshop")]
            CareersManagementWorkshop = 6,
            [Display(Name = "Apply For Apprenticeship")]
            ApplyForApprenticeship = 7,
            [Display(Name = "Apply For Traineeship")]
            ApplyForTraineeship = 8,
            [Display(Name = "Attend Skills Fair Or Skills Show")]
            AttendSkillsFairOrSkillsShow = 9,
            [Display(Name = "Volunteer")]
            Volunteer = 10,
            [Display(Name = "Use National Careers Service Website")]
            UseNationalCareersServiceWebsite = 11,
            [Display(Name = "Use External Digital Services")]
            UseExternalDigitalServices = 12,
            [Display(Name = "Book Follow Up Appointment")]
            BookFollowUpAppointment = 13,
            [Display(Name = "Use Social Media")]
            UseSocialMedia = 14,
            [Display(Name = "Discovery Your Skills And Careers")]
            DiscoveryYourSkillsAndCareers = 15,
            [Display(Name = "Find A Course")]
            FindACourse = 16,
            [Display(Name = "Other")]
            Other = 99

        }

}
