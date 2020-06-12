using System;
using System.Collections.Generic;
using System.Text;
using DFC.App.ActionPlans.Models;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests
{
    class Models
    {
        class CompositeSettingsModelTests
        {
            [Test]
            public void CompositeSettingsModel()
            {
                var compositeSettings = new CompositeSettings();
                compositeSettings.Path = "SomePath";
                compositeSettings.CDN = "SomeCDN";
            }
        }

        class GoalModelTests
        {
            [Test]
            public void GoalModel()
            {
                var goal = new Goal()
                {
                    ActionPlanId = "Id",
                    CustomerId = "cId",
                    DateGoalAchieved = DateTime.Now,
                    DateGoalCaptured = DateTime.Now,
                    DateGoalShouldBeCompletedBy = DateTime.Now,
                    GoalId = "gId",
                    GoalStatus = 1,
                    GoalSummary = "summary",
                    GoalType=1,
                    LastModifiedBy = "me",
                    LastModifiedDate = DateTime.Now,
                    SubcontractorId = "sId"
                };
                
            }
        }
    }
}
