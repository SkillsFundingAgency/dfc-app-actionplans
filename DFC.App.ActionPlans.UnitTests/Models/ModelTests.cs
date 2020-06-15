using System;
using DFC.App.ActionPlans.Models;
using DFC.App.ActionPlans.Services.DSS.Enums;
using NUnit.Framework;

namespace DFC.App.ActionPlans.UnitTests
{
    class ModelTests
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
                    GoalStatus = GoalStatus.Achieved,
                    GoalSummary = "summary",
                    GoalType=GoalType.Learning,
                    LastModifiedBy = "me",
                    LastModifiedDate = DateTime.Now,
                    SubcontractorId = "sId"
                };
                
            }
        }

        class AuthSettingsModelTests
        {
            [Test]
            public void AuthSettingsModel()
            {
                var authSettings = new AuthSettings()
                {
                    ClientId = "id",
                    ClientSecret = "secret",
                    Issuer = "isuser",
                    RegisterUrl = "url",
                    ResetPasswordUrl = "url",
                    SignInUrl = "url",
                    SignOutUrl = "url"
                };

            }
        }
    }
}
