using System;

namespace DFC.App.ActionPlans.ViewModels
{
    public class UpdateGoalConfirmationCompositeViewModel : CompositeViewModel 
    {
        public UpdateGoalConfirmationCompositeViewModel() : base(PageId.Error, "")
        {
        }

        public int ObjectUpdated { get; set; }
        public int PropertyUpdated { get; set; }
        public String WhatChanged { get; set; }
        public String Name { get; set; }
        public String UpdateMessage { get; set; }
        public String ObjLink { get; set; }
        public String ObjLinkText { get; set; }
    }
}
