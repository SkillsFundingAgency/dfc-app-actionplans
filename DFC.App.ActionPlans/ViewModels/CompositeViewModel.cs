using System;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Models;
using DFC.Personalisation.Common.Extensions;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.ActionPlans.ViewModels
{
    public abstract class CompositeViewModel
    {
        public static string AppTitle => "Action plans";
        public static string NcsBranding => "National Careers Service";

        public class PageId
        {
            private PageId(string value)
            {
                Throw.IfNullOrWhiteSpace(value, nameof(value));
                Value = value.Trim();
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }

            public static PageId Home { get; } = new PageId("home");
            public static PageId Error { get; } = new PageId("error");
            public static PageId ViewGoal { get; } = new PageId("view-goal");
            public static PageId ChangeGoalDueDate { get; } = new PageId("change-goal-due-date");
            public static PageId ChangeGoalStatus { get; } = new PageId("change-goal-status");
            public static PageId ChangeActionDueDate { get; } = new PageId("change-action-due-date");
            public static PageId ChangeActionStatus { get; } = new PageId("change-action-status");
            public static PageId ViewAction { get; } = new PageId("view-action");
            public static PageId UpdateConfirmation { get; } = new PageId("update-confirmation");
        }

        public class PageRegion
        {
            private PageRegion(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }

            public string Value { get; }
            public static PageRegion Body { get; } = new PageRegion("body");
        }

        protected CompositeViewModel(PageId pageId, string pageHeading)
        {
            Id = pageId;
            PageHeading = pageHeading;
            GeneratePageTitle(pageHeading);
        }

        public void GeneratePageTitle(string pageHeading)
        {
            PageTitle = string.IsNullOrWhiteSpace(pageHeading) ? $"{AppTitle} | {NcsBranding}" : $"{pageHeading} | {AppTitle} | {NcsBranding}";
        }

        public PageId Id { get; }
        public string PageTitle { get; set;}
        public string PageHeading { get; set;}
        public string Name { get; set; }
        public string BackLink { get; set; }
        public bool ShowBreadCrumb { get; set; }
        public CompositeSettings CompositeSettings { get; set; }
        public Guid CustomerId { get; set;}
        public Guid ActionPlanId { get; set;}
        public Guid InteractionId { get; set;}
        public Interaction Interaction { get; set;}
        public Adviser Adviser { get; set;}
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();
        public string SharedContent { get; set; }


        public string GetElementId(string elementName, string instanceName)
        {
            Throw.IfNullOrWhiteSpace(elementName, nameof(elementName));
            Throw.IfNullOrWhiteSpace(instanceName, nameof(instanceName));
            elementName = elementName.FirstCharToUpper().Trim();
            instanceName = instanceName.FirstCharToUpper().Trim();
            return $"{Id}{elementName}{instanceName}";
        }
    }
}

