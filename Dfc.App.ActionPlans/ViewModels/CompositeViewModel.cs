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
            public static PageId ViewAction { get; } = new PageId("view-action");

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

        public PageId Id { get; }

        public string PageTitle { get; set;}
        public string PageHeading { get; set;}
        public string Name { get; set; }

        public bool ShowBreadCrumb { get; set; }
        public CompositeSettings CompositeSettings { get; set; }

        protected CompositeViewModel(PageId pageId, string pageHeading)
        {
            Id = pageId;
            PageHeading = pageHeading;
            
            PageTitle = string.IsNullOrWhiteSpace(pageHeading) ? $"{AppTitle} | {NcsBranding}" : $"{pageHeading} | {AppTitle} | {NcsBranding}";
        }

        public Guid CustomerId { get; set;}
        public Guid ActionPlanId { get; set;}
        public Guid InteractionId { get; set;}
        public Session LatestSession { get; set;}
        public Interaction Interaction { get; set;}
        public Adviser Adviser { get; set;}
        public ContactDetails ContactDetails { get; set; } = new ContactDetails();
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

