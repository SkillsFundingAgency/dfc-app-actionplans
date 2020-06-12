﻿using DFC.App.ActionPlans.Models;
using Dfc.ProviderPortal.Packages;

namespace DFC.App.ActionPlans.ViewModels
{
    public abstract class CompositeViewModel
    {
        public static string AppTitle => "Action plan";
        public static string NCSBranding => "National Careers Service";

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
            public static PageId ChangePassword { get; } = new PageId("change-password");


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
            
            PageTitle = string.IsNullOrWhiteSpace(pageHeading) ? $"{AppTitle} | {NCSBranding}" : $"{pageHeading} | {AppTitle} | {NCSBranding}";
        }

      
    }
}

